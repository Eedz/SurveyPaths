using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITCLib;

namespace SurveyPaths
{
    static class UserTypeDefs
    {
        public static Respondent CreateBlankRespondent()
        {
            Respondent r = new Respondent();
            r.Survey = "";
            r.Description = "Blank Respondent";

            return r;
        }

        public static Respondent CreateKRA1TripleUser()
        {
            Respondent r = new Respondent();
            r.Survey = "KRA1";
            r.Description = "Triple User";

            r.AddResponse("FR309v", "01");
            r.AddResponse("EC309v", "01");
            r.AddResponse("HN309v", "01");
            return r;
        }

        public static Respondent CreateKRA1_SM_EC()
        {
            Respondent r = new Respondent();
            r.Survey = "KRA1";
            r.Description = "Smoker Ecig user";

            r.AddResponse("FR309v", "01");
            r.AddResponse("EC309v", "01");
            r.AddResponse("HN309v", "06");
           
            return r;
        }

        public static Respondent CreateKRA1_SM_HN()
        {
            Respondent r = new Respondent();
            r.Survey = "KRA1";
            r.Description = "Smoker HTP User";

            r.AddResponse("FR309v", "01");
            r.AddResponse("EC309v", "06");
            r.AddResponse("HN309v", "01");
            return r;
        }

        public static Respondent CreateKRA1_EC_HN()
        {
            Respondent r = new Respondent();
            r.Survey = "KRA1";
            r.Description = "Ecig HTP User";

            r.AddResponse("FR309v", "09");
            r.AddResponse("EC309v", "01");
            r.AddResponse("HN309v", "01");
            return r;
        }

        public static Respondent CreateKRA1_EC()
        {
            Respondent r = new Respondent();
            r.Survey = "KRA1";
            r.Description = "Ecig only";

            r.AddResponse("FR309v", "09");
            r.AddResponse("EC309v", "01");
            r.AddResponse("HN309v", "06");

            return r;
        }

        public static Respondent CreateKRA1_HN()
        {
            Respondent r = new Respondent();
            r.Survey = "KRA1";
            r.Description = "HTP User only";

            r.AddResponse("FR309v", "09");
            r.AddResponse("EC309v", "06");
            r.AddResponse("HN309v", "01");
            return r;
        }

        public static Respondent CreateKRA1_SM()
        {
            Respondent r = new Respondent();
            r.Survey = "KRA1";
            r.Description = "Smoker only";


            r.AddResponse("FR309v", "01");
            r.AddResponse("EC309v", "06");
            r.AddResponse("HN309v", "06");

            return r;
        }

        public static Respondent CreateSpanishRespondent()
        {
            Respondent r = new Respondent();
            r.Survey = "6E2";
            r.Description = "Spanish Respondent";
            r.AddResponse("BI101", "6", false);
            return r;
        }

        public static Respondent CreateESUT1()
        {
            Respondent r = new Respondent();
            r.Survey = "ES2.5";
            r.Description = "Smoker Heard of Ecig";

            r.AddResponse("FR309v", "1", false);
            //r.AddResponse("FR309v", "2", false);
            //r.AddResponse("FR309v", "3", false);
            //r.AddResponse("FR309v", "4", false);
            r.AddResponse("FR305", "1");
            r.AddResponse("NC302", "1", false);
            r.AddResponse("NC302", "2", false);

            r.AddResponse("Rtype", "C");

            return r;
        }

        public static Respondent CreateESUT2()
        {
            Respondent r = new Respondent();
            r.Survey = "ES2.5";
            r.Description = "Smoker Not Heard of Ecig";

            r.AddResponse("FR309v", "1", false);
           // r.AddResponse("FR309v", "2", false);
           // r.AddResponse("FR309v", "3", false);
           // r.AddResponse("FR309v", "4", false);
            r.AddResponse("FR305", "1");
            r.AddResponse("NC302", "3", false);
            r.AddResponse("NC302", "8", false);
            r.AddResponse("NC302", "9", false);

            r.AddResponse("Rtype", "C");

            return r;
        }

        public static Respondent CreateESUT3()
        {
            Respondent r = new Respondent();
            r.Survey = "ES2.5";
            r.Description = "Quitter Heard of Ecig";

            r.AddResponse("FR309v", "5", false);
            r.AddResponse("FR309v", "6", false);
            r.AddResponse("FR309v", "7", false);
            r.AddResponse("FR305", "2");
            r.AddResponse("NC302", "1", false);
            r.AddResponse("NC302", "2", false);

            r.AddResponse("Rtype", "C");

            return r;
        }

        public static Respondent CreateESUT4()
        {
            Respondent r = new Respondent();
            r.Survey = "ES2.5";
            r.Description = "Quitter Not Heard of Ecig";

            r.AddResponse("FR309v", "5", false);
            r.AddResponse("FR309v", "6", false);
            r.AddResponse("FR309v", "7", false);
            r.AddResponse("FR305", "2");
            r.AddResponse("NC302", "3", false);
            r.AddResponse("NC302", "8", false);
            r.AddResponse("NC302", "9", false);

            r.AddResponse("Rtype", "C");

            return r;
        }

        public static Respondent Create6E2UT1()
        {
            Respondent r = new Respondent();
            r.Survey = "6E2";
            r.Description = "Smoker Heard of Ecig (C)";
            r.AddResponse("FR305", "1", false);
            r.AddResponse("FR225", "1", false);
            //r.AddResponse("FR225", "2", false);
            //r.AddResponse("FR225", "3", false);
            //r.AddResponse("FR225", "4", false);
            r.AddResponse("QA211", "1");
            r.AddResponse("NC301", "1");

            r.AddResponse("Rtype", "C");

            //r.AddResponse("SM938", "2");

            return r;
        }

        

        public static Respondent Create6E2UT2()
        {
            Respondent r = new Respondent();
            r.Survey = "6E2";
            r.Description = "Smoker Not Heard of Ecig";

            r.AddResponse("FR225", "1", false);
            //r.AddResponse("FR225", "2", false);
            //r.AddResponse("FR225", "3", false);
            //r.AddResponse("FR225", "4", false);
            r.AddResponse("QA211", "1");
            r.AddResponse("FR305", "1", false);
            r.AddResponse("NC301", "2", false);
            r.AddResponse("NC301", "8", false);
            r.AddResponse("NC301", "9", false);
            r.AddResponse("Rtype", "C");


            return r;
        }

        public static Respondent Create6E2UT3()
        {
            Respondent r = new Respondent();
            r.Survey = "6E2";
            r.Description = "Quitter Heard of Ecig";

            r.AddResponse("QA211", "2");
            r.AddResponse("FR305", "2", false);
            r.AddResponse("NC301", "1");
            r.AddResponse("Rtype", "C");


            return r;
        }

        public static Respondent Create6E2UT4()

        {
            Respondent r = new Respondent();
            r.Survey = "6E2";
            r.Description = "Quitter Not Heard of Ecig";

            r.AddResponse("QA211", "2");
            r.AddResponse("FR305", "2", false);
            r.AddResponse("NC301", "2", false);
            r.AddResponse("NC301", "8", false);
            r.AddResponse("NC301", "9", false);

            r.AddResponse("Rtype", "C");

            return r;
        }
        public static Respondent CreateNZL3SmokerNonVaper()
        {
            Respondent r = new Respondent();
            r.Survey = "NZL3";
            r.Description = "Daily Smoker NonVaper";

            r.AddResponse("FR309v", "1");

            r.Responses.Add(new Answer("EC309v", "5"));

            r.AddResponse("Rtype", "C");
            r.AddResponse("Rsource", "1");
            r.AddResponse("NC302", "1");
            r.AddResponse("NC309", "3");
            r.AddResponse("NC304", "5");

            r.AddResponse("FR225", "1");
            r.AddResponse("FR225v", "1");

            return r;
        }

        public static Respondent CreateNZL3NonSmokerNonVaper()
        {
            Respondent r = new Respondent();
            r.Survey = "NZL3";
            r.Description = "NonSmoker NonVaper";

            r.Responses.Add(new Answer("FR309v", "7"));

            r.Responses.Add(new Answer("EC309v", "5"));

            //r.AddResponse("NT042", "1");
            r.AddResponse("BI203", "1");
            //r.AddResponse("NT043", "1");
            r.AddResponse("DE098", "1");
            r.AddResponse("BI350", "2");
            r.AddResponse("FR142", "1");
            // r.AddResponse("NT044", "1");
            r.AddResponse("QA437", "1");
            r.AddResponse("FR305", "2");
            r.AddResponse("FR225", "5");
            r.AddResponse("QA439", "5");

            r.AddResponse("Rtype", "C");
            r.AddResponse("Rsource", "1");
            r.AddResponse("NC302", "1");
            r.AddResponse("NC309", "5");
            r.AddResponse("NC304", "5");

            return r;
        }

        public static Respondent CreateNZL3NonSmokerVaper()
        {
            Respondent r = new Respondent();
            r.Survey = "NZL3";
            r.Description = "NonSmoker Vaper";

            r.AddResponse("FR309v", "7");

            r.AddResponse("EC309v", "1");

            r.AddResponse("Rtype", "P");
            r.AddResponse("Rsource", "2");
            //r.AddResponse("NT042", "1");
            r.AddResponse("BI203", "1");
            //r.AddResponse("NT043", "1");
            r.AddResponse("DE098", "1");
            r.AddResponse("BI350", "2");
            r.AddResponse("FR142", "1");
            // r.AddResponse("NT044", "1");
            r.AddResponse("QA437", "1");
            r.AddResponse("FR305", "2");
            r.AddResponse("FR225", "5");
            r.AddResponse("FR225v", "5");
            r.AddResponse("QA439", "5");

            return r;
        }

        public static Respondent CreateNZL3SmokerVaper()
        {
            Respondent r = new Respondent();
            r.Survey = "NZL3";
            r.Description = "Daily Smoker Vaper";

            r.AddResponse("Rsource", "1");
            r.AddResponse("Rtype", "C");
            r.AddResponse("BI200", "20");
            r.AddResponse("FR225", "1");
            r.AddResponse("FR225v", "1");
            r.AddResponse("FR309v", "1"); // daily smoker
            r.AddResponse("NC302", "1");
            r.AddResponse("NC309", "1");
            r.AddResponse("NC304", "1");
            r.AddResponse("EC309v", "1"); // daily vaper

            return r;
        }

        public static Respondent CreateMYS1NonSmokerNonEcig()
        {
            Respondent r = new Respondent();
            r.Survey = "MYS1";
            r.Description = "Non-Smoker & Combined Ecig non-user";

            r.Responses.Add(new Answer("FR225", "5"));
            r.Responses.Add(new Answer("FR225", "6"));
            r.AddResponse("FR309v", "9");
            r.AddResponse("FR305", "2");
            r.AddResponse("NC302v", "2");
            r.AddResponse("NC301v", "1");

            r.Responses.Add(new Answer("NC304", "4"));
            r.Responses.Add(new Answer("NC304", "5"));
            r.Responses.Add(new Answer("NC304", "8"));
            r.Responses.Add(new Answer("NC304", "9"));

            return r;
        }

        public static Respondent CreateMYS1SmokerCombinedNonEcig()
        {
            Respondent r = new Respondent();
            r.Survey = "MYS1";
            r.Description = "Smoker & Combined Ecig non-user";

            r.AddResponse("FR225", "1");
            r.AddResponse("FR309v", "1");
            r.AddResponse("FR305", "1");
            r.AddResponse("NC302v", "2");
            r.AddResponse("NC301v", "1");

            r.Responses.Add(new Answer("NC304", "4"));
            r.Responses.Add(new Answer("NC304", "5"));
            r.Responses.Add(new Answer("NC304", "8"));
            r.Responses.Add(new Answer("NC304", "9"));

            return r;
        }

        public static Respondent CreateMYS1SmokerCombinedEcig()
        {
            Respondent r = new Respondent();
            r.Survey = "MYS1";
            r.Description = "Smoker & Combined Ecig user";

            r.AddResponse("FR225", "1");
            r.AddResponse("FR309v", "1");
            r.AddResponse("FR305", "1");
            r.AddResponse("NC302v", "1");
            r.AddResponse("NC301v", "1");

            r.Responses.Add(new Answer("NC304", "1"));
            r.Responses.Add(new Answer("NC304", "2"));
            r.Responses.Add(new Answer("NC304", "3"));

            return r;
        }

        // MYS1
        public static Respondent CreateMYS1SimpleSmoker()
        {
            Respondent r = new Respondent();
            r.Survey = "MYS1";
            r.Description = "Simple Smoker";
            r.AddResponse("FR225", "1");
            r.AddResponse("FR309v", "1");
            r.AddResponse("FR305", "1");


            return r;
        }

        public static Respondent CreateMYS1SimpleNonSmoker()
        {
            Respondent r = new Respondent();
            r.Survey = "MYS1";
            r.Description = "Simple Non-Smoker";
            r.AddResponse("FR225", "6");
            r.AddResponse("FR309v", "9");
            r.AddResponse("FR305", "2");


            return r;
        }

        public static Respondent CreateMYS1UT1()
        {
            Respondent r = new Respondent();
            r.Survey = "MYS1";
            r.Description = "Daily smoker, daily E-cig user, not using  HTPs, Made QA  ";
            r.AddResponse("FR225", "1");
            r.AddResponse("FR309v", "1");
            r.AddResponse("FR305", "1");

            r.AddResponse("NC304", "1");
            r.AddResponse("HN140", "1");
            r.AddResponse("QA101", "1");
            r.AddResponse("QA561", "1");
            return r;
        }

        public static Respondent CreateMYS1UT2()
        {
            Respondent r = new Respondent();
            r.Survey = "MYS1";
            r.Description = "Monthly smoker, not using e-cigs, never used HTPs, no QA";
            r.AddResponse("FR225", "3");
            r.AddResponse("FR309v", "3");
            r.AddResponse("FR305", "1");

            r.AddResponse("NC304", "5");
            r.AddResponse("HN106v", "2");
            r.AddResponse("QA101", "2");
            r.AddResponse("QA561", "0");
            return r;
        }

        public static Respondent CreateMYS1UT3()
        {
            Respondent r = new Respondent();
            r.Survey = "MYS1";
            r.Description = "Non-smoker";
            r.AddResponse("FR225", "6");
            r.AddResponse("FR309v", "9");
            r.AddResponse("FR305", "2");
            return r;
        }




        // NZL2
        public static Respondent CreateUT1Max()
        {
            // UT1

            Respondent r = new Respondent();
            r.Survey = "NZL2";
            r.AddResponse("Rtype", "P");
            r.AddResponse("FR309v", "1");
            r.AddResponse("NC302v", "1");
            r.AddResponse("NC304", "1");
            r.AddResponse("HN103", "1");
            r.AddResponse("HN106", "1");
            r.AddResponse("EC375", "1");
            r.AddResponse("QA561v", "1");
            r.AddResponse("QA442v", "90");

            r.AddResponse("EQ101", "1");
            r.AddResponse("BR310", "1");
            r.AddResponse("DE111", "1");

            return r;
        }

        public static Respondent CreateUT1Min()
        {
            // UT1

            Respondent r = new Respondent();
            r.Survey = "NZL2";
            r.AddResponse("Rtype", "C");
            r.AddResponse("FR309v", 1);
            r.AddResponse("NC302v", 1);
            r.AddResponse("NC304", 1);
            r.AddResponse("HN103", 2);
            //r.AddResponse("HN106", 1);
            r.AddResponse("EC375", 3);
            r.AddResponse("QA561v", 2);
            //r.AddResponse("QA442v", 90);
            r.AddResponse("EQ101", 2);
            r.AddResponse("BR310", 2);
            r.AddResponse("DE111", 6);

            return r;
        }

        public static Respondent CreateUT2Max()
        {
            // UT2

            Respondent r = new Respondent();
            r.Survey = "NZL2";
            r.AddResponse("Rtype", "P");
            r.AddResponse("FR309v", "1");
            r.AddResponse("NC302v", "2");
            //r.AddResponse("NC304", 1);
            r.AddResponse("HN103", "1");
            r.AddResponse("HN106", "1");
            //r.AddResponse("EC375", 3);
            r.AddResponse("QA561v", "1");
            r.AddResponse("QA442v", "90");
            r.AddResponse("EQ101", "1");
            r.AddResponse("BR310", "1");
            r.AddResponse("DE111", "1");

            return r;
        }

        public static Respondent CreateUT2Min()
        {
            // UT2

            Respondent r = new Respondent();
            r.Survey = "NZL2";
            r.AddResponse("Rtype", "C");
            r.AddResponse("FR309v", "1");
            r.AddResponse("NC302v", "2");
            //r.AddResponse("NC304", 1);
            r.AddResponse("HN103", "2");
            //r.AddResponse("HN106", 1);
            //r.AddResponse("EC375", 3);
            r.AddResponse("QA561v", "2");
            //r.AddResponse("QA442v", 90);
            r.AddResponse("EQ101", "2");
            r.AddResponse("BR310", "2");
            r.AddResponse("DE111", "6");

            return r;
        }

        public static Respondent CreateUT3Max()
        {
            // UT3

            Respondent r = new Respondent();
            r.Survey = "NZL2";
            r.AddResponse("Rtype", "P");
            r.AddResponse("FR309v", "1");
            r.AddResponse("NC302v", "1");
            r.AddResponse("NC304", "5");
            r.AddResponse("HN103", "1");
            r.AddResponse("HN106", "1");
            //r.AddResponse("EC375", 3);
            r.AddResponse("QA561v", "1");
            r.AddResponse("QA442v", "90");
            r.AddResponse("EQ101", "1");
            r.AddResponse("BR310", "1");
            r.AddResponse("DE111", "1");

            return r;
        }

        public static Respondent CreateUT3Min()
        {
            // UT3

            Respondent r = new Respondent();
            r.Survey = "NZL2";
            r.AddResponse("Rtype", "C");
            r.AddResponse("FR309v", "1");
            r.AddResponse("NC302v", "1");
            r.AddResponse("NC304", "5");
            r.AddResponse("HN103", "2");
            //r.AddResponse("HN106", 1);
            //r.AddResponse("EC375", 3);
            r.AddResponse("QA561v", "2");
            //r.AddResponse("QA442v", 90);
            r.AddResponse("EQ101", "2");
            r.AddResponse("BR310", "2");
            r.AddResponse("DE111", "6");

            return r;
        }

        public static Respondent CreateUT4Max()
        {
            // UT4

            Respondent r = new Respondent();
            r.Survey = "NZL2";
            r.AddResponse("Rtype", "P");
            r.AddResponse("FR309v", "4");
            r.AddResponse("NC302v", "1");
            r.AddResponse("NC304", "1");
            r.AddResponse("HN103", "1");
            r.AddResponse("HN106", "1");
            r.AddResponse("EC375", "1");
            //r.AddResponse("QA561v", 1);
            r.AddResponse("QA442v", "90");
            r.AddResponse("EQ101", "1");
            r.AddResponse("BR310", "1");
            r.AddResponse("DE111", "1");

            return r;
        }

        public static Respondent CreateUT4Min()
        {
            // UT4

            Respondent r = new Respondent();
            r.Survey = "NZL2";
            r.AddResponse("Rtype", "C");
            r.AddResponse("FR309v", "6");
            r.AddResponse("NC302v", "1");
            r.AddResponse("NC304", "1");
            r.AddResponse("HN103", "2");
            //r.AddResponse("HN106", 1);
            r.AddResponse("EC375", "3");
            //r.AddResponse("QA561v", 1);
            r.AddResponse("QA442v", "200");
            r.AddResponse("EQ101", "2");
            r.AddResponse("BR310", "2");
            r.AddResponse("DE111", "6");

            return r;
        }

        public static Respondent CreateUT5Max()
        {
            // UT5

            Respondent r = new Respondent();
            r.Survey = "NZL2";
            r.AddResponse("Rtype", "P");
            r.AddResponse("FR309v", "4");
            r.AddResponse("NC302v", "2");
            //r.AddResponse("NC304", 1);
            r.AddResponse("HN103", "1");
            r.AddResponse("HN106", "1");
            //r.AddResponse("EC375", 3);
            //r.AddResponse("QA561v", 1);
            r.AddResponse("QA442v", "90");
            r.AddResponse("EQ101", "1");
            r.AddResponse("BR310", "1");
            r.AddResponse("DE111", "1");

            return r;
        }

        public static Respondent CreateUT5Min()
        {
            // UT5

            Respondent r = new Respondent();
            r.Survey = "NZL2";
            r.AddResponse("Rtype", "C");
            r.AddResponse("FR309v", "6");
            r.AddResponse("NC302v", "2");
            //r.AddResponse("NC304", 1);
            r.AddResponse("HN103", "2");
            //r.AddResponse("HN106", 1);
            //r.AddResponse("EC375", 3);
            //r.AddResponse("QA561v", 1);
            r.AddResponse("QA442v", "200");
            r.AddResponse("EQ101", "2");
            r.AddResponse("BR310", "2");
            r.AddResponse("DE111", "6");

            return r;
        }
    }
}
