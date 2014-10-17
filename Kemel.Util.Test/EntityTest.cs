using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Util.Test
{
    public class EntityTest
    {
        private string strName = null;
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(strName))
                {
                    string lastName = Names.GetLastName();
                    strName = string.Concat(Names.GetFirstName(Gender), " ", lastName);
                    strFather = string.Concat(Names.GetFirstName(Names.Gender.Male), " ", lastName);
                    strMother = string.Concat(Names.GetFirstName(Names.Gender.Female), " ", lastName);
                }
                return strName;
            }
        }

        private string strFather = null;
        public string Father
        {
            get
            {
                if (string.IsNullOrEmpty(strFather))
                    this.Name.ToString();
                return strFather;
            }
        }

        private string strMother = null;
        public string Mother
        {
            get
            {
                if (string.IsNullOrEmpty(strMother))
                    this.Name.ToString();
                return strMother;
            }
        }

        private Kemel.Util.Test.Names.Gender? enmGender = null;
        public Kemel.Util.Test.Names.Gender Gender
        {
            get
            {
                if (!enmGender.HasValue)
                    enmGender = (Kemel.Util.Test.Names.Gender)Randonizer.NextInt(2);
                return enmGender.Value;
            }
        }

        private DateTime? dtBirthdate = null;
        public DateTime Birthdate
        {
            get
            {
                if (!dtBirthdate.HasValue)
                    dtBirthdate = Others.GenerateDate();
                return dtBirthdate.Value;
            }
        }

        public int Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int tsAge = today.Year - Birthdate.Year;
                if (Birthdate > today.AddYears(-tsAge)) tsAge--;
                return tsAge;
            }
        }

        private string strEMail = null;
        public string EMail
        {
            get
            {
                if (string.IsNullOrEmpty(strEMail))
                    strEMail = Others.GenerateEmail();
                return strEMail;
            }
        }

        private string strWebSite = null;
        public string WebSite
        {
            get
            {
                if (string.IsNullOrEmpty(strWebSite))
                    strWebSite = Others.GenerateUrl();
                return strWebSite;
            }
        }

        private string strStreet = null;
        public string Street
        {
            get
            {
                if (string.IsNullOrEmpty(strStreet))
                    strStreet = Address.GenerateStreetName();
                return strStreet;
            }
        }

        private int? intHouseNumber = null;
        public int HouseNumber
        {
            get
            {
                if (!intHouseNumber.HasValue)
                    intHouseNumber = Randonizer.NextInt(1, 11000);
                return intHouseNumber.Value;
            }
        }

        private string strDistrict = null;
        public string District
        {
            get
            {
                if (string.IsNullOrEmpty(strDistrict))
                    strDistrict = Address.GenerateDistrictName();
                return strDistrict;
            }
        }

        private States? sttState = null;
        public States State
        {
            get
            {
                if (!sttState.HasValue)
                    sttState = Address.GetRandomState();
                return sttState.Value;
            }
        }

        public string StateAcronym
        {
            get
            {
                return State.ToString();
            }
        }

        public string StateName
        {
            get
            {
                return Address.GetStateName(State);
            }
        }

        private string strCity = null;
        public string City
        {
            get
            {
                if (string.IsNullOrEmpty(strCity))
                    strCity = Address.GetAnyCityFromState(State);
                return strCity;
            }
        }

        private string strZipCode = null;
        public string ZipCode
        {
            get
            {
                if (string.IsNullOrEmpty(strZipCode))
                    strZipCode = Address.GenerateZipNumber();
                return strZipCode;
            }
        }

        private string strCompanyName = null;
        public string CompanyName
        {
            get
            {
                if (string.IsNullOrEmpty(strCompanyName))
                    strCompanyName = Names.GetCompanyName();
                return strCompanyName;
            }
        }

        private string strCPF = null;
        public string CPF
        {
            get
            {
                if (string.IsNullOrEmpty(strCPF))
                    strCPF = Documents.GenerateCPF();
                return strCPF;
            }
        }

        private string strCNPJ = null;
        public string CNPJ
        {
            get
            {
                if (string.IsNullOrEmpty(strCNPJ))
                    strCNPJ = Documents.GenerateCNPJ();
                return strCNPJ;
            }
        }

        private int? intDDD = null;
        public int DDD
        {
            get
            {
                if (!intDDD.HasValue)
                    intDDD = Telephone.GetAnyDDDFromState(State);
                return intDDD.Value;
            }
        }

        private string strTelephone = null;
        public string TelephoneNumber
        {
            get
            {
                if (string.IsNullOrEmpty(strTelephone))
                    strTelephone = Telephone.GenerateCompleteTelephoneNumberFromDDD(DDD);
                return strTelephone;
            }
        }

        private string strCellphone = null;
        public string CellphoneNumber
        {
            get
            {
                if (string.IsNullOrEmpty(strCellphone))
                    strCellphone = Telephone.GenerateCompleteCellphoneNumberFromDDD(DDD);
                return strCellphone;
            }
        }


    }
}
