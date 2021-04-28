using System.Globalization;

namespace Interface.Scripts
{
    public class LightColor
    {
        public LightColor()
        {
            RedValue = 0;
            GreenValue = 0;
            BlueValue = 0;
        }

        public LightColor(string hexColor)
        {
            RedValue = byte.Parse(hexColor.Substring(1, 2), NumberStyles.HexNumber);
            GreenValue = byte.Parse(hexColor.Substring(3, 2), NumberStyles.HexNumber);
            BlueValue = byte.Parse(hexColor.Substring(5, 2), NumberStyles.HexNumber);
        }

        public LightColor(int intColor)
        {
            string hexColor = intColor.ToString("X6");
            RedValue = byte.Parse(hexColor.Substring(0, 2), NumberStyles.HexNumber);
            GreenValue = byte.Parse(hexColor.Substring(2, 2), NumberStyles.HexNumber);
            BlueValue = byte.Parse(hexColor.Substring(4, 2), NumberStyles.HexNumber);
        }

        private byte _redValue;
        private byte _greenValue;
        private byte _blueValue;

        public string Color { get; private set; }

        public byte RedValue
        {
            get => _redValue;
            set
            {
                _redValue = value;
                OnValueChanged();
            }
        }

        public byte GreenValue
        {
            get => _greenValue;
            set
            {
                _greenValue = value;
                OnValueChanged();
            }
        }

        public byte BlueValue
        {
            get => _blueValue;
            set
            {
                _blueValue = value;
                OnValueChanged();
            }
        }

        private void OnValueChanged()
        {
            Color = "#" + _redValue.ToString("X2")
                        + _greenValue.ToString("X2")
                        + _blueValue.ToString("X2");
        }

        public int GetIntColor()
        {
            string hexColor = Color.Substring(1, 6);
            return int.Parse(hexColor, NumberStyles.HexNumber);
        }
    }
}