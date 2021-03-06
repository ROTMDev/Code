﻿// =Realms Engine=
// =Realms Of the Mind=
// =Programmers=
// =Mute Lovestone=
// =ContentBuilder.cs=
// = 11/3/2014 =
// =ROTM_MU002=
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
namespace ROTM_MU002.Windows.Builder
{
    public class ContentBuilder : IDisposable
    {
        #region Fields
        // What importers or processors should we load?
        const string xnaVersion = ", Version=4.0.0.0, PublicKeyToken=842cf8be1de50553";
        static string[] pipelineAssemblies = {
            string.Format("Microsoft.Xna.Framework.Content.Pipeline.FBXImporter{0}", xnaVersion),
            string.Format("Microsoft.Xna.Framework.Content.Pipeline.XImporter{0}", xnaVersion),

            // If you want to use custom importers or processors from
            // a Content Pipeline Extension Library, add them here.
            //
            // If your extension DLL is installed in the GAC, you should refer to it by assembly
            // name, eg. "MyPipelineExtension, Version=1.0.0.0, PublicKeyToken=1234567812345678".
            //
            // If the extension DLL is not in the GAC, you should refer to it by
            // file path, eg. "c:/MyProject/bin/MyPipelineExtension.dll".
        };
        // MSBuild objects used to dynamically build content.
        Project buildProject;
        ProjectRootElement projectRootElement;
        BuildParameters buildParameters;
        List<ProjectItem> projectItems = new List<ProjectItem>();
        // Temporary directories used by the content build.
        string buildDirectory;
        string processDirectory;
        string baseDirectory;
        // Generate unique directory names if there is more than one ContentBuilder.
        static int directorySalt;
        // Have we been disposed?
        bool isDisposed;
        class ImporterDef
        {
            public string SupportedExtension;
            public string Importer;
            public string Processor;
            public ImporterDef(string extension, string imp, string proc)
            {
                this.SupportedExtension = extension;
                this.Importer = imp;
                this.Processor = proc;
            }
        }
        class ImporterGroup
        {
            List<ImporterDef> Importers = new List<ImporterDef>();
            public void Add(ImporterDef importer)
            { this.Importers.Add(importer); }
            public ImporterDef FindByExtension(string extension)
            {
                for (int i = 0; i < this.Importers.Count; i++)
                {
                    if (this.Importers[i].SupportedExtension.Equals(extension))
                    { return this.Importers[i]; }
                }
                return null;
            }
        }
        private ImporterGroup Importers;
        #endregion
        #region Properties
        /// <summary>
        /// Gets the output directory, which will contain the generated .xnb files.
        /// </summary>
        public string OutputDirectory { get; set; }
        #endregion
        #region Initialization
        /// <summary>
        /// Creates a new content builder.
        /// </summary>
        public ContentBuilder(string outs)
        {
            this.OutputDirectory = outs;
            this.CreateTempDirectory();
            this.CreateBuildProject();
            this.Importers = new ImporterGroup();
            this.Importers.Add(new ImporterDef(".fx", "EffectImporter", "EffectProcessor"));
            this.Importers.Add(new ImporterDef(".fbx", "FbxImporter", "ModelProcessor"));
        }
        /// <summary>
        /// Finalizes the content builder.
        /// </summary>
        ~ContentBuilder()
        { this.Dispose(false); }
        /// <summary>
        /// Disposes the content builder when it is no longer required.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Implements the standard .NET IDisposable pattern.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                this.isDisposed = true;
                
                this.DeleteTempDirectory();
            }
        }
        #endregion
        #region MSBuild
        /// <summary>
        /// Creates a temporary MSBuild content project in memory.
        /// </summary>
        void CreateBuildProject()
        {
            string projectPath = Path.Combine(this.buildDirectory, "");
            string outputPath = Path.Combine(this.buildDirectory, "");
            
            // Create the build project.
            this.projectRootElement = ProjectRootElement.Create(this.OutputDirectory);
            
            // Include the standard targets file that defines how to build XNA Framework content.
            this.projectRootElement.AddImport("$(MSBuildExtensionsPath)\\Microsoft\\XNA Game Studio\\v4.0\\Microsoft.Xna.GameStudio.ContentPipeline.targets");
            
            this.buildProject = new Project(this.projectRootElement);
            
            this.buildProject.SetProperty("XnaPlatform", "Windows");
            this.buildProject.SetProperty("XnaProfile", "HiDef");
            this.buildProject.SetProperty("XnaFrameworkVersion", "v4.0");
            this.buildProject.SetProperty("Configuration", "Release");
            this.buildProject.SetProperty("OutputPath", this.OutputDirectory);
            
            string workingDir = Directory.GetCurrentDirectory();
            
            // Register any custom importers or processors.
            foreach (string pipelineAssembly in pipelineAssemblies)
            {
                string assemblyReferencePath = pipelineAssembly;
                if (pipelineAssembly.EndsWith(".dll"))
                {
                    assemblyReferencePath = Path.GetFileName(pipelineAssembly);
                    File.Copy(Path.Combine(workingDir, pipelineAssembly), Path.Combine(this.buildDirectory, assemblyReferencePath));
                }
                this.buildProject.AddItem("Reference", assemblyReferencePath);
            }
            
            // Hook up our custom error logger.
            
            this.buildParameters = new BuildParameters(ProjectCollection.GlobalProjectCollection);
        }
        public void Add(string filepath)
        {
            var importer = this.Importers.FindByExtension(Path.GetExtension(filepath).ToLower());
            this.Add(filepath, Path.GetFileNameWithoutExtension(filepath), importer.Importer, importer.Processor);
        }
        /// <summary>
        /// Adds a new content file to the MSBuild project. The importer and
        /// processor are optional: if you leave the importer null, it will
        /// be autodetected based on the file extension, and if you leave the
        /// processor null, data will be passed through without any processing.
        /// </summary>
        public void Add(string filename, string name, string importer, string processor)
        {
            ProjectItem item = this.buildProject.AddItem("Compile", filename)[0];
            
            item.SetMetadataValue("Link", Path.GetFileName(filename));
            item.SetMetadataValue("Name", name);
            
            if (!string.IsNullOrEmpty(importer))
            { item.SetMetadataValue("Importer", importer); }
            
            if (!string.IsNullOrEmpty(processor))
            { item.SetMetadataValue("Processor", processor); }
            
            this.projectItems.Add(item);
        }
        /// <summary>
        /// Removes all content files from the MSBuild project.
        /// </summary>
        public void Clear()
        {
            this.buildProject.RemoveItems(this.projectItems);
            
            this.projectItems.Clear();
        }
        /// <summary>
        /// Builds all the content files which have been added to the project,
        /// dynamically creating .xnb files in the OutputDirectory.
        /// Returns an error message if the build fails.
        /// </summary>
        public string Build()
        {
            // Clear any previous errors.
            //errorLogger.Errors.Clear();
            // Create and submit a new asynchronous build request.
            BuildManager.DefaultBuildManager.BeginBuild(this.buildParameters);
            
            BuildRequestData request = new BuildRequestData(this.buildProject.CreateProjectInstance(), new string[0]);
            BuildSubmission submission = BuildManager.DefaultBuildManager.PendBuildRequest(request);
            
            submission.ExecuteAsync(null, null);
            
            // Wait for the build to finish.
            submission.WaitHandle.WaitOne();
            
            BuildManager.DefaultBuildManager.EndBuild();
            
            // If the build failed, return an error string.
            if (submission.BuildResult.OverallResult == BuildResultCode.Failure)
            { return "\n"; }

            return null;
        }
        #endregion
        #region Temp Directories
        /// <summary>
        /// Creates a temporary directory in which to build content.
        /// </summary>
        void CreateTempDirectory()
        {
            // Start with a standard base name:
            //
            //  %temp%\WinFormsContentLoading.ContentBuilder
            this.baseDirectory = Path.Combine(Path.GetTempPath(), this.GetType().FullName);
            
            // Include our process ID, in case there is more than
            // one copy of the program running at the same time:
            //
            //  %temp%\WinFormsContentLoading.ContentBuilder\<ProcessId>
            
            int processId = Process.GetCurrentProcess().Id;

            this.processDirectory = Path.Combine(this.baseDirectory, processId.ToString());
            
            // Include a salt value, in case the program
            // creates more than one ContentBuilder instance:
            //
            //  %temp%\WinFormsContentLoading.ContentBuilder\<ProcessId>\<Salt>

            directorySalt++;

            this.buildDirectory = Path.Combine(this.processDirectory, directorySalt.ToString());
            
            // Create our temporary directory.
            Directory.CreateDirectory(this.buildDirectory);

            this.PurgeStaleTempDirectories();
        }
        /// <summary>
        /// Deletes our temporary directory when we are finished with it.
        /// </summary>
        void DeleteTempDirectory()
        {
            Directory.Delete(this.buildDirectory, true);
            
            // If there are no other instances of ContentBuilder still using their
            // own temp directories, we can delete the process directory as well.
            if (Directory.GetDirectories(this.processDirectory).Length == 0)
            {
                Directory.Delete(this.processDirectory);
                
                // If there are no other copies of the program still using their
                // own temp directories, we can delete the base directory as well.
                if (Directory.GetDirectories(this.baseDirectory).Length == 0)
                { Directory.Delete(this.baseDirectory); }
            }
        }
        /// <summary>
        /// Ideally, we want to delete our temp directory when we are finished using
        /// it. The DeleteTempDirectory method (called by whichever happens first out
        /// of Dispose or our finalizer) does exactly that. Trouble is, sometimes
        /// these cleanup methods may never execute. For instance if the program
        /// crashes, or is halted using the debugger, we never get a chance to do
        /// our deleting. The next time we start up, this method checks for any temp
        /// directories that were left over by previous runs which failed to shut
        /// down cleanly. This makes sure these orphaned directories will not just
        /// be left lying around forever.
        /// </summary>
        void PurgeStaleTempDirectories()
        {
            // Check all subdirectories of our base location.
            foreach (string directory in Directory.GetDirectories(this.baseDirectory))
            {
                // The subdirectory name is the ID of the process which created it.
                int processId;
                
                if (int.TryParse(Path.GetFileName(directory), out processId))
                {
                    try
                    { Process.GetProcessById(processId); }
                    catch (ArgumentException)
                    { Directory.Delete(directory, true); }
                }
            }
        }
        #endregion
    }
}