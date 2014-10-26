// =Realms Engine=
// =Realms Of the Mind=
// =Programmers=
// =Mute Lovestone=
// =AttributePair.cs=
// = 10/25/2014 =
// =MUlib=

namespace LibRealm.Base
{
    public class AttributePair
    {
        #region Field Region
        
        int currentValue;
        int maximumValue;
        #endregion
        #region Property Region
        
        public int CurrentValue
        {
            get { return this.currentValue; }
        }
        
        public int MaximumValue
        {
            get { return this.maximumValue; }
        }
        
        public static AttributePair Zero
        {
            get { return new AttributePair(); }
        }
        #endregion
        #region Constructor Region
        
        private AttributePair()
        {
            this.currentValue = 0;
            this.maximumValue = 0;
        }
        
        public AttributePair(int maxValue)
        {
            this.currentValue = maxValue;
            this.maximumValue = maxValue;
        }
        #endregion
        #region Method Region
        
        public void Heal(ushort value)
        {
            this.currentValue += value;
            if (this.currentValue > this.maximumValue)
            { this.currentValue = this.maximumValue; }
        }
        
        public void Damage(ushort value)
        {
            this.currentValue -= value;
            if (this.currentValue < 0)
            { this.currentValue = 0; }
        }
        
        public void SetCurrent(int value)
        {
            this.currentValue = value;
            if (this.currentValue > this.maximumValue)
            { this.currentValue = this.maximumValue; }
        }
        
        public void SetMaximum(int value)
        {
            this.maximumValue = value;
            if (this.currentValue > this.maximumValue)
            { this.currentValue = this.maximumValue; }
        }
        #endregion
    }
}