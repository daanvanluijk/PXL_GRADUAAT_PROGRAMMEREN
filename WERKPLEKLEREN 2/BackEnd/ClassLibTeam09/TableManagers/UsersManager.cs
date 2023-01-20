using ClassLibTeam09.Data.Framework;
using ClassLibTeam09.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClassLibTeam09.TableManagers
{
    public static class UsersManager
    {
        // Dictionary met initialen voor elke user property
        // De array van grootte drie bevat eerst de overeenkomende parameternaam, 
        // dan de klasse-property en dan de kolomnaam vanuit de tabel in de database
        //
        // Als we nu eens overal de zelfde naamgeving gebruikten, dan was deze joekel niet nodig
        public static readonly Dictionary<string, string[]> lookup = new Dictionary<string, string[]>()
        {
            ["ID"] = new string[3] { "@userID", "UserId", "userID" },
            ["PW"] = new string[3] { "@password", "Password", "Password" },
            ["LN"] = new string[3] { "@lastname", "LastName", "lastName" },
            ["FN"] = new string[3] { "@firstname", "FirstName", "firstName" },
            ["C"] = new string[3] { "@country", "Country", "country" },
            ["E"] = new string[3] { "@email", "Email", "email" },
            ["A"] = new string[3] { "@adress", "Adress", "adress" },
            ["PN"] = new string[3] { "@placename", "PlaceName", "placeName" },
            ["ZC"] = new string[3] { "@zipcode", "Zipcode", "zipcode" },
            ["P"] = new string[3] { "@phone", "Phone", "phone" },
        };


        // Elke procedure roept een procedure op op basis van de method naam ("InsertUser" roept in de database "InsertUser" op)
        // Het is dus cruciaal dat de method naam exact overeenkomt met de stored procedure in de database
        //
        // De method BaseProcedure neemt als tweede parameter een lijst van parameters die in de stored procedure nodig zijn
        // Deze lijst wordt meegegeven als een string waarbij de parameter initialen gescheiden zijn door een komma (geen spaties!)
        // Zie lookup dictionary bovenaan
        //
        // Voorbeeld method: 
        // public static [type]Result [vb: SelectUser](User user)
        //    => BaseManager.BaseProcedure(user, [vb: "ID,PW,.."], Procedures.OperationType.[type], lookup);
        #region Procedures
        public static SelectResult SelectUsers()
            => BaseManager.BaseProcedure(Procedures.OperationType.Select, lookup);

        public static SelectResult SelectUserWhereUserIDAndPassword(User user)
            => BaseManager.BaseProcedure(user, "ID,PW", Procedures.OperationType.Select, lookup);

        public static SelectResult SelectUserWhereEmailAndPassword(User user)
            => BaseManager.BaseProcedure(user, "E,PW", Procedures.OperationType.Select, lookup);

        public static SelectResult SelectAmountOfEmailsWhereEmail(User user)
            => BaseManager.BaseProcedure(user, "E", Procedures.OperationType.Select, lookup);

        public static SelectResult SelectUserWhereEmail(User user)
            => BaseManager.BaseProcedure(user, "E", Procedures.OperationType.Select, lookup);

        public static UpdateResult UpdateUserWithPasswordWhereUserID(User user)
            => BaseManager.BaseProcedure(user, "ID,PW,LN,FN,C,E,A,PN,ZC,P", Procedures.OperationType.Update, lookup);

        public static UpdateResult UpdateUserWhereUserID(User user)
            => BaseManager.BaseProcedure(user, "ID,LN,FN,C,E,A,PN,ZC,P", Procedures.OperationType.Update, lookup);

        public static UpdateResult UpdateUserPasswordWhereUserID(User user)
            => BaseManager.BaseProcedure(user, "PW", Procedures.OperationType.Update, lookup);

        public static InsertResult InsertUser(User user)
            => BaseManager.BaseProcedure(user, "PW,LN,FN,C,E,A,PN,ZC,P", Procedures.OperationType.Insert, lookup);

        public static DeleteResult DeleteUserWhereUserID(User user)
            => BaseManager.BaseProcedure(user, "ID", Procedures.OperationType.Delete, lookup);
        #endregion

        // Neemt een tabel en converteert deze naar een User object op basis van de lookup dictionary
        public static User ConvertDataRowToUser(DataTable table)
            => BaseManager.ConvertTableToObject<User>(table, lookup);

        // User Checks
        public static ValidateResult ValidateUser(User user, bool needsPassword = true)
        {
            ValidateResult validateResult = new ValidateResult();

            // Check voor leeg of null of whitespace
            if (string.IsNullOrWhiteSpace(user.LastName)
                || string.IsNullOrWhiteSpace(user.FirstName)
                || string.IsNullOrWhiteSpace(user.Email)
                || string.IsNullOrWhiteSpace(user.Adress)
                || string.IsNullOrWhiteSpace(user.PlaceName)
                || string.IsNullOrWhiteSpace(user.Zipcode)
                || string.IsNullOrWhiteSpace(user.Phone))
            {
                validateResult.Succeeded = false;
                validateResult.AddError("niet alle velden waren ingevuld");
                return validateResult;
            }
            if (needsPassword && string.IsNullOrWhiteSpace(user.Password))
            {
                validateResult.Succeeded = false;
                validateResult.AddError("niet alle velden waren ingevuld");
                return validateResult;
            }

            // Check lengte
            string[] propsToCheck = { "LastName", "FirstName", "Email", "Adress", "PlaceName", "Phone" };
            foreach (string propName in propsToCheck)
            {
                var prop = user.GetType().GetProperty(propName);
                string sProp = (string)prop.GetValue(user);
                if (sProp.Length > 50)
                {
                    validateResult.Succeeded = false;
                    validateResult.AddError($"{propName} was te lang");
                    return validateResult;
                }
            }
            if (needsPassword && user.Password.Length > 50)
            {
                validateResult.Succeeded = false;
                validateResult.AddError($"Password was te lang");
                return validateResult;
            }
            if (user.Zipcode.Length > 10)
            {
                validateResult.Succeeded = false;
                validateResult.AddError("Zipcode was te lang");
                return validateResult;
            }

            // Regex checks
            propsToCheck = new string[] { "Phone" };
            Regex regex = new Regex(@"^[0-9]+$");
            foreach (string propName in propsToCheck)
            {
                var prop = user.GetType().GetProperty(propName);
                string sProp = (string)prop.GetValue(user);
                if (!regex.IsMatch(sProp))
                {
                    validateResult.Succeeded = false;
                    validateResult.AddError($"{propName} bevatte niet alleen cijfers");
                    return validateResult;
                }
            }
            propsToCheck = new string[] { "LastName", "FirstName", "PlaceName" };
            regex = new Regex(@"\\PL");
            foreach (string propName in propsToCheck)
            {
                var prop = user.GetType().GetProperty(propName);
                string sProp = (string)prop.GetValue(user);
                if (regex.IsMatch(sProp))
                {
                    validateResult.Succeeded = false;
                    validateResult.AddError($"{propName} bevatte cijfers");
                    return validateResult;
                }
            }
            regex = new Regex(@"[0-9]");
            if (!regex.IsMatch(user.Adress))
            {
                validateResult.Succeeded = false;
                validateResult.AddError("Adress bevatte geen cijfers");
                return validateResult;
            }
            regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$");
            if (!regex.IsMatch(user.Email))
            {
                validateResult.Succeeded = false;
                validateResult.AddError("Email was niet geldig");
                return validateResult;
            }

            validateResult.Succeeded = true;
            return validateResult;
        }

        // Normalises a User
        public static User NormaliseUser(User user)
        {
            // Trim
            string[] propsToCheck = new string[] { "LastName", "FirstName", "Email", "Adress", "PlaceName", "Zipcode", "Phone" };
            foreach (string propName in propsToCheck)
            {
                var prop = user.GetType().GetProperty(propName);
                string sProp = (string)prop.GetValue(user);
                prop.SetValue(user, sProp.Trim());
            }

            // Remove commas
            propsToCheck = new string[] { "Adress" };
            foreach (string propName in propsToCheck)
            {
                var prop = user.GetType().GetProperty(propName);
                string sProp = (string)prop.GetValue(user);
                prop.SetValue(user, sProp.Replace(",", ""));
            }

            // Lowercase
            propsToCheck = new string[] { "Email" };
            foreach (string propName in propsToCheck)
            {
                var prop = user.GetType().GetProperty(propName);
                string sProp = (string)prop.GetValue(user);
                prop.SetValue(user, sProp.ToLower());
            }

            return user;
        }
    }
}
