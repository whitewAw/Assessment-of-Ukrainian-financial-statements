namespace AFS.Core.Models
{
    public class Form1
    {
        public BeginEnd F1001 { get; set; } = new();
        public BeginEnd F1002 { get; set; } = new();
        public BeginEnd F1005 { get; set; } = new();
        public BeginEnd F1011 { get; set; } = new();
        public BeginEnd F1012 { get; set; } = new();
        public BeginEnd F1015 { get; set; } = new();
        public BeginEnd F1020 { get; set; } = new();
        public BeginEnd F1030 { get; set; } = new();
        public BeginEnd F1035 { get; set; } = new();
        public BeginEnd F1040 { get; set; } = new();
        public BeginEnd F1045 { get; set; } = new();
        public BeginEnd F1090 { get; set; } = new();
        public BeginEnd F1101 { get; set; } = new();
        public BeginEnd F1102 { get; set; } = new();
        public BeginEnd F1103 { get; set; } = new();
        public BeginEnd F1104 { get; set; } = new();
        public BeginEnd F1110 { get; set; } = new();
        public BeginEnd F1120 { get; set; } = new();
        public BeginEnd F1125 { get; set; } = new();
        public BeginEnd F1130 { get; set; } = new();
        public BeginEnd F1135 { get; set; } = new();
        public BeginEnd F1136 { get; set; } = new();
        public BeginEnd F1140 { get; set; } = new();
        public BeginEnd F1145 { get; set; } = new();
        public BeginEnd F1155 { get; set; } = new();
        public BeginEnd F1160 { get; set; } = new();
        public BeginEnd F1165 { get; set; } = new();
        public BeginEnd F1170 { get; set; } = new();
        public BeginEnd F1190 { get; set; } = new();
        public BeginEnd F1200 { get; set; } = new();
        public BeginEnd F1400 { get; set; } = new();
        public BeginEnd F1405 { get; set; } = new();
        public BeginEnd F1410 { get; set; } = new();
        public BeginEnd F1415 { get; set; } = new();
        public BeginEnd F1420 { get; set; } = new();
        public BeginEnd F1425 { get; set; } = new();
        public BeginEnd F1430 { get; set; } = new();
        public BeginEnd F1500 { get; set; } = new();
        public BeginEnd F1510 { get; set; } = new();
        public BeginEnd F1515 { get; set; } = new();
        public BeginEnd F1520 { get; set; } = new();
        public BeginEnd F1525 { get; set; } = new();
        public BeginEnd F1600 { get; set; } = new();
        public BeginEnd F1605 { get; set; } = new();
        public BeginEnd F1610 { get; set; } = new();
        public BeginEnd F1615 { get; set; } = new();
        public BeginEnd F1620 { get; set; } = new();
        public BeginEnd F1621 { get; set; } = new();
        public BeginEnd F1625 { get; set; } = new();
        public BeginEnd F1630 { get; set; } = new();
        public BeginEnd F1635 { get; set; } = new();
        public BeginEnd F1640 { get; set; } = new();
        public BeginEnd F1645 { get; set; } = new();
        public BeginEnd F1660 { get; set; } = new();
        public BeginEnd F1665 { get; set; } = new();
        public BeginEnd F1690 { get; set; } = new();
        public BeginEnd F1700 { get; set; } = new();

        private BeginEnd[] GetAllFieldsArray() =>
        [
            F1001, F1002, F1005, F1011, F1012, F1015, F1020, F1030, F1035, F1040, F1045, F1090,
            F1101, F1102, F1103, F1104, F1110, F1120, F1125, F1130, F1135, F1136, F1140, F1145,
            F1155, F1160, F1165, F1170, F1190, F1200, F1400, F1405, F1410, F1415, F1420, F1425,
            F1430, F1500, F1510, F1515, F1520, F1525, F1600, F1605, F1610, F1615, F1620, F1621,
            F1625, F1630, F1635, F1640, F1645, F1660, F1665, F1690, F1700
        ];

        internal void Init(Form1 form1)
        {
            var thisFields = GetAllFieldsArray();
            var sourceFields = form1.GetAllFieldsArray();
            for (int i = 0; i < thisFields.Length; i++)
                thisFields[i].Init(sourceFields[i]);
        }

        internal void SubscribeOnChange(Action propertyChanged)
        {
            foreach (var field in GetAllFieldsArray())
                field.PropertyChanged += propertyChanged;
        }

        internal void UnSubscribeOnChange(Action? propertyChanged)
        {
            if (propertyChanged == null) return;
            foreach (var field in GetAllFieldsArray())
                field.PropertyChanged -= propertyChanged;
        }

        public double GetF1000Begin() => F1001.Begin - F1002.Begin;
        public double GetF1000End() => F1001.End - F1002.End;
        public double GetF1010Begin() => F1011.Begin - F1012.Begin;
        public double GetF1010End() => F1011.End - F1012.End;
        public double GetF1095Begin() => GetF1000Begin() + F1005.Begin + GetF1010Begin() + F1015.Begin + F1020.Begin + F1030.Begin + F1035.Begin + F1040.Begin + F1045.Begin + F1090.Begin;
        public double GetF1095End() => GetF1000End() + F1005.End + GetF1010End() + F1015.End + F1020.End + F1030.End + F1035.End + F1040.End + F1045.End + F1090.End;
        public double GetF1100Begin() => F1101.Begin + F1102.Begin + F1103.Begin + F1104.Begin;
        public double GetF1100End() => F1101.End + F1102.End + F1103.End + F1104.End;
        public double GetF1195Begin() => GetF1100Begin() + F1110.Begin + F1120.Begin + F1125.Begin + F1130.Begin + F1135.Begin + F1140.Begin + F1145.Begin + F1155.Begin + F1160.Begin + F1165.Begin + F1170.Begin + F1190.Begin;
        public double GetF1195End() => GetF1100End() + F1110.End + F1120.End + F1125.End + F1130.End + F1135.End + F1140.End + F1145.End + F1155.End + F1160.End + F1165.End + F1170.End + F1190.End;
        public double GetF1300Begin() => GetF1095Begin() + GetF1195Begin() + F1200.Begin;
        public double GetF1300End() => GetF1095End() + GetF1195End() + F1200.End;
        public double GetF1495Begin() => F1400.Begin + F1405.Begin + F1410.Begin + F1415.Begin + F1420.Begin - F1425.Begin - F1430.Begin;
        public double GetF1495End() => F1400.End + F1405.End + F1410.End + F1415.End + F1420.End - F1425.End - F1430.End;
        public double GetF1595Begin() => F1500.Begin + F1510.Begin + F1515.Begin + F1520.Begin + F1525.Begin;
        public double GetF1595End() => F1500.End + F1510.End + F1515.End + F1520.End + F1525.End;
        public double GetF1695Begin() => F1600.Begin + F1605.Begin + F1610.Begin + F1615.Begin + F1620.Begin + F1625.Begin + F1630.Begin + F1635.Begin + F1640.Begin + F1645.Begin + F1660.Begin + F1665.Begin + F1690.Begin;
        public double GetF1695End() => F1600.End + F1605.End + F1610.End + F1615.End + F1620.End + F1625.End + F1630.End + F1635.End + F1640.End + F1645.End + F1660.End + F1665.End + F1690.End;
        public double GetF1900Begin() => GetF1495Begin() + GetF1595Begin() + GetF1695Begin() + F1700.Begin;
        public double GetF1900End() => GetF1495End() + GetF1595End() + GetF1695End() + F1700.End;

        internal double GetAccountsTangibleAssets(bool begin)
        {
            if (begin)
            {
                return GetF1100Begin() + F1110.Begin + F1200.Begin;
            }
            return GetF1100End() + F1110.End + F1200.End;
        }
        internal double GetAccountsReceivable(bool begin)
        {
            if (begin)
            {
                return F1120.Begin + F1125.Begin + F1130.Begin + F1135.Begin + F1140.Begin + F1145.Begin + F1155.Begin;
            }
            return F1120.End + F1125.End + F1130.End + F1135.End + F1140.End + F1145.End + F1155.End;
        }
        internal double GetAccountsMoney(bool begin)
        {
            if (begin)
            {
                return F1160.Begin + F1165.Begin;
            }
            return F1160.End + F1165.End;
        }
        internal double GetProvisionOfNextCostsAndPayments(bool begin)
        {
            if (begin)
            {
                return F1520.Begin + F1525.Begin;
            }
            return F1520.End + F1525.End;
        }
        internal double GetAccountsPayable(bool begin)
        {
            if (begin)
            {
                return F1605.Begin + F1615.Begin + F1620.Begin + F1625.Begin + F1630.Begin + F1635.Begin + F1640.Begin + F1645.Begin;
            }
            return F1605.End + F1615.End + F1620.End + F1625.End + F1630.End + F1635.End + F1640.End + F1645.End;
        }
    }
}
