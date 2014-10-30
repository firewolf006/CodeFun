using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Net;
using System.Net.Http;
using HtmlAgilityPack;
using Newtonsoft.Json;
using ProjectManagement_API.Models.DTO;
using Content_Centre_API_CORS.Messaging;
using Content_Centre_API.Messaging;

namespace Importinator
{
    partial class importer : ServiceBase
    {
        
        public importer()
        {
            InitializeComponent();
        }

        public async void import(string[] args)
        {
            //ServiceBase.Run(new ServiceBase[]
            //    {
            //        new importer()
            //    });

            // TODO: Add code here to start your service.
            String lineS = "";
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            Console.WriteLine("Getting token");

            HttpClient client = new HttpClient();
            string OscToken = "Bearer " + await GetAuthToken();
            OscToken = OscToken.Replace("#access_token=", "");
            client.DefaultRequestHeaders.Add("Authorization", OscToken);

            Console.WriteLine("token Retrieved");

            StreamReader reader = File.OpenText(args[0]);
            lineS = reader.ReadToEnd();


            //using (StreamReader reader = File.OpenText(args[0]))
            //    while (!reader.EndOfStream)
            //    {
            //        //string[] temp;
            //        string line = reader.ReadLine();
            //        if (null == line)
            //            continue;
            //        lineS += line;

            //        reader.
            //    }

            clientDTO testClient = new clientDTO();
            projectDTO proJ = new projectDTO();
            responseObject resultObject = new responseObject();
            ProjectManagementRequest reQ = new ProjectManagementRequest();

            testClient.name = "Paul Thorott";
            testClient.email = "Cloud.Strife@ffxiv.pol";
            testClient.phoneNumber = "1.123.123.1234";

            Console.WriteLine("some text....");

            proJ.client = testClient;
            proJ.documents = new List<documentDTO>();
            proJ.resources = new List<resourceDTO>();
            proJ.invoices = new List<invoiceDTO>();
            proJ.projectName = "What What?";

            documentDTO tempDoc = new documentDTO();
            resourceDTO tempRes = new resourceDTO();
            invoiceDTO tempInv = new invoiceDTO();
            languageDTO tempLan = new languageDTO();

            tempLan.translateDirectionID = new List<int>();
            tempLan.translateDirectionID.Add(3);
            tempLan.translateDirectionID.Add(2);            

            tempDoc.name = "DOC1";
            tempDoc.filePath = "x:\\sick.jpg";
            tempDoc.status = 1;
            tempDoc.sourceLanguageID = 1;
            tempDoc.translateDirections = new languageDTO();
            tempDoc.translateDirections = tempLan;


            tempDoc.typeID = 1;

            tempRes.firstName = "Don";
            tempRes.lastName = "Matroska";
            tempRes.company = "OSC";
            tempRes.address = "100 disney drive";
            tempRes.type = "0";
            tempRes.isCompany = false;
            tempRes.languageAssociations = new languageDTO();
            tempRes.languageAssociations = tempLan;
            tempRes.languageAssociations.translateDirectionID[0] = 1;
            tempRes.languageAssociations.translateDirectionID[1] = 3;
            tempRes.documents = new List<documentDTO>();

            tempInv.price = 0.50;
            tempInv.status = "1";
            tempInv.version = 1.00;
            tempInv.documentID = 344;

            proJ.invoices.Add(tempInv);
            proJ.documents.Add(tempDoc);
            proJ.resources.Add(tempRes);


            reQ.projects = new List<projectDTO>();

            reQ.projects.Add(proJ);

            Console.WriteLine("some more text....");

            //String ajaxURI = "https://localhost/ProjectManagement-API/api/projectmanagement/postProject/";



            //HttpResponseMessage httpResponse = client.PostAsJsonAsync<ProjectManagementRequest>(ajaxURI, reQ).Result;
            //resultObject = httpResponse.Content.ReadAsAsync<responseObject>().Result;


            String ajaxURI = ConfigurationManager.AppSettings["SpecWebApi"];

            theList test = new theList();
            test = JsonConvert.DeserializeObject<theList>(lineS);


            // -----------Get meal types
            MealTypeTransferObject reQ1 = new MealTypeTransferObject();
            List<MealTypeTransferObject> MealTypes = new List<MealTypeTransferObject>();

            HttpResponseMessage httpResponse = client.PostAsJsonAsync<MealTypeTransferObject>(ajaxURI + "/getAllMealTypes", reQ1).Result;
            Console.WriteLine("\n|--------------------|");
            Console.WriteLine("... Getting Meal Type IDs");
            Console.WriteLine("Data Sent.. did it work? ....");

            if (httpResponse.StatusCode == (HttpStatusCode)200)
            {
                Console.WriteLine("yes....yes it did");
                MealTypeResponseObject PressXtoJSON = new MealTypeResponseObject();
                PressXtoJSON = await httpResponse.Content.ReadAsAsync<MealTypeResponseObject>();
                MealTypes = JsonConvert.DeserializeObject<List<MealTypeTransferObject>>(PressXtoJSON.JsonResults);
                Console.WriteLine("MealTypes populated");
            }
            else
            {
                Console.WriteLine("nope, wrong again!");
                Console.WriteLine("--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase);
            }
            


            // ---------Get Status Codes
            StatusTransferObject reQ2 = new StatusTransferObject();
            List<StatusTransferObject> SpecStatuses = new List<StatusTransferObject>();

            httpResponse = client.PostAsJsonAsync<StatusTransferObject>(ajaxURI + "/getAllStatuses", reQ2).Result;
            Console.WriteLine("\n|--------------------|");
            Console.WriteLine("... Getting Status IDs");
            Console.WriteLine("Data Sent.. did it work? ....");

            if (httpResponse.StatusCode == (HttpStatusCode)200)
            {
                Console.WriteLine("yes....yes it did");
                StatusResponseObject PressXtoJSON = new StatusResponseObject();
                PressXtoJSON = await httpResponse.Content.ReadAsAsync<StatusResponseObject>();
                SpecStatuses = JsonConvert.DeserializeObject<List<StatusTransferObject>>(PressXtoJSON.JsonResults);
                Console.WriteLine("Statuses populated");
            }
            else
            {
                Console.WriteLine("nope, wrong again!");
                Console.WriteLine("--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase);
            }

            // -----------Get rotations
            RotationGrillTransferObject reQ3 = new RotationGrillTransferObject();
            List<RotationGrillTransferObject> Rotations = new List<RotationGrillTransferObject>();

            httpResponse = client.PostAsJsonAsync<RotationGrillTransferObject>(ajaxURI + "/getRotationByName", reQ3).Result;
            Console.WriteLine("\n|--------------------|");
            Console.WriteLine("... Rotation Grill IDs");
            Console.WriteLine("Data Sent.. did it work? ....");

            if (httpResponse.StatusCode == (HttpStatusCode)200)
            {
                Console.WriteLine("yes....yes it did");
                RotationGrillResponseObject PressXtoJSON = new RotationGrillResponseObject();
                PressXtoJSON = await httpResponse.Content.ReadAsAsync<RotationGrillResponseObject>();
                Rotations = JsonConvert.DeserializeObject<List<RotationGrillTransferObject>>(PressXtoJSON.JsonResults);
                Console.WriteLine("Rotations populated");
            }
            else
            {
                Console.WriteLine("nope, wrong again!");
                Console.WriteLine("--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase);
            }

            // -----------Get languages
            LanguageTransferObject reQ4 = new LanguageTransferObject();
            List<LanguageTransferObject> Languages = new List<LanguageTransferObject>();

            httpResponse = client.PostAsJsonAsync<LanguageTransferObject>(ajaxURI + "/getAllLanguages", reQ4).Result;
            Console.WriteLine("\n|--------------------|");
            Console.WriteLine("... Rotation Grill IDs");
            Console.WriteLine("Data Sent.. did it work? ....");

            if (httpResponse.StatusCode == (HttpStatusCode)200)
            {
                Console.WriteLine("yes....yes it did");
                LanguageResponseObject PressXtoJSON = new LanguageResponseObject();
                PressXtoJSON = await httpResponse.Content.ReadAsAsync<LanguageResponseObject>();
                Languages = JsonConvert.DeserializeObject<List<LanguageTransferObject>>(PressXtoJSON.JsonResults);
                Console.WriteLine("Languages populated");
            }
            else
            {
                Console.WriteLine("nope, wrong again!");
                Console.WriteLine("--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase);
            }

            // -----------Get allergens....
            Console.WriteLine("\n|--------------------|");
            Console.WriteLine("... getting Allergen IDs");
            Console.WriteLine("NOPE...... just kidding");

            List<AllergenTransferObject> Allergens = new List<AllergenTransferObject>();

            AllergenTransferObject tempAlly = new AllergenTransferObject();

            tempAlly.AllergenID = 0;
            tempAlly.Value = "Fish";
            Allergens.Add(tempAlly);
            
            tempAlly.AllergenID = 1;
            tempAlly.Value = "Egg";
            Allergens.Add(tempAlly);

            tempAlly.AllergenID = 2;
            tempAlly.Value = "Gluten";
            Allergens.Add(tempAlly);

            tempAlly.AllergenID = 3;
            tempAlly.Value = "Bees";
            Allergens.Add(tempAlly);

            tempAlly.AllergenID = 4;
            tempAlly.Value = "Milk";
            Allergens.Add(tempAlly);

            // ------------------------////////////////////////

            COURSE pole = new COURSE();
            List<ALLERGEN> oool = new List<ALLERGEN>();
            ALLERGEN looo = new ALLERGEN();


            looo.allergen = "milk";
            oool.Add(looo);

            looo.allergen = "Egg";
            oool.Add(looo);

            looo.allergen = "BeeS";
            oool.Add(looo);

            pole.allergens = oool;

            String PressMtoJSON = "";
            //PressMtoJSON = JsonConvert.DeserializeObject<theList>(lineS);
            PressMtoJSON = JsonConvert.SerializeObject(pole);

            // display some of the specs for verification
            String str = "";
            int start = 0, howMany = 9; //9
            for (int xList = start; xList < howMany + start; xList++)
            {
                DataList temp = new DataList();
                SpecTransferObject newSpec = new SpecTransferObject();

                temp = test.menuDataList[xList];

                //newSpec.AccountID = "1"; // Air France

                newSpec.Rotation = temp.cycle;
                newSpec.ValidDate = temp.validityBegin;
                newSpec.CatererName = temp.caterer;


                //until parsed is fixed

                if (temp.state == null)
                    temp.state = "Proofing";



                // Check if spec exists





                //String codeString = "", stationString = "";

                // build code string
                for (int x = 0; x < temp.mealList.Count; x++)
                {
                    newSpec.Code += temp.mealList[x].code;
                    if (x + 1 != temp.mealList.Count)
                        newSpec.Code += "/";
                }

                // build class string
                for (int x = 0; x < temp.classCodeList.Count; x++)
                {
                    newSpec.Class += temp.classCodeList[x].code;
                    if (x + 1 != temp.classCodeList.Count)
                        newSpec.Class += "/";
                }

                // build station string
                for (int x = 0; x < temp.departureArrivalList.Count; x++)
                {
                    newSpec.Station += temp.departureArrivalList[x].departure.code;
                    if (temp.departureArrivalList[x].arrival != null)
                        newSpec.Station += "(" + temp.departureArrivalList[x].arrival.code + ")";
                    if (x + 1 != temp.departureArrivalList.Count)
                        newSpec.Station += "/";
                }

                // get MealType
                newSpec.MealTypeID = getMealTypeID(MealTypes, temp.mealType);

                // get Status Code
                newSpec.StatusID = getStatusID(SpecStatuses, temp.state);

                // Get Rotation ID?
                newSpec.RotationID = getRotationGrillID(Rotations, temp.pairingGrid);

                // ----------- insert NEW and Get Spec ID .......

                //SpecTransferObject reQ5 = new SpecTransferObject();
                //List<LanguageTransferObject> Languages = new List<LanguageTransferObject>();


                httpResponse = client.PostAsJsonAsync<SpecTransferObject>(ajaxURI + "/insertSpec", newSpec).Result;
                Console.WriteLine("\n|--------------------|");
                Console.WriteLine("... Getting Spec ID for new Spec");
                Console.WriteLine("Data Sent.. did it work? ....");

                if (httpResponse.StatusCode == (HttpStatusCode)200)
                {
                    Console.WriteLine("yes....yes it did");
                    SpecResponseObject PressXtoJSON = new SpecResponseObject();
                    try
                    {
                        PressXtoJSON = await httpResponse.Content.ReadAsAsync<SpecResponseObject>();
                        if (PressXtoJSON.Error)
                        {
                            Console.WriteLine("the faiL, has arrived :(");
                            foreach (Content_Centre_API_CORS.ErrorHandling.SpecificationError m in PressXtoJSON.ErrorList)
                            {
                                Console.WriteLine("-> " + m.ErrorMessage);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Spec was a GREAT SUCCESS");
                            newSpec.SpecID = PressXtoJSON.SpecID;
                        }
                    }
                    catch (Newtonsoft.Json.JsonSerializationException e)
                    {
                        Console.WriteLine("the faiL, has arrived :(");
                        Console.WriteLine(e.Message);
                    }
                    //SpecRes = JsonConvert.DeserializeObject<List<SpecTransferObject>>(PressXtoJSON.JsonResults);

                }
                else
                {
                    Console.WriteLine("nope, wrong again!");
                    Console.WriteLine("--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase);
                }

                List<SpecificationItemTransferObject> courses = new List<SpecificationItemTransferObject>();

                int headerCounter = 1, subItemCounter = 1;

                for (int x = 0; x < temp.menusList[0].coursesList.Count; x++)
                {
                    COURSE m = temp.menusList[0].coursesList[x];
                    SpecificationItemTransferObject tempCourse = new SpecificationItemTransferObject();
                    int xID = x + 1;

                    // get Allergen IDs
                    //foreach (ALLERGEN m2 in m.allergens)
                    //{

                    //}

                    tempCourse.SpecificationItemID = -(courses.Count + 1);
                    tempCourse.LanguageID = getLanguageID(Languages, temp.language);
                    tempCourse.SpecificationID = newSpec.SpecID;

                    if (m.label != null)
                    {
                        tempCourse.ParentID = 0; // this is a header
                        tempCourse.Value = m.label;
                        tempCourse.OrderNumber = headerCounter++;
                        subItemCounter = 1;
                    }
                    else
                    {
                        if (x > 0)
                        {
                            int parentX = 0;
                            while (courses[(courses.Count - 1) - parentX].ParentID != 0)
                            {
                                parentX++;
                            }
                            tempCourse.ParentID = courses[(courses.Count - 1) - parentX].SpecificationItemID;
                            tempCourse.Value = m.content;
                            tempCourse.OrderNumber = subItemCounter++;
                        }
                    }



                    courses.Add(tempCourse);

                    // Check for children
                    if (m.label != null)
                    {
                        if (m.content != "") // feed the children
                        {
                            SpecificationItemTransferObject courseRei = new SpecificationItemTransferObject();
                            //xID = x + 2;
                            courseRei.SpecificationItemID = -(courses.Count + 1);
                            courseRei.LanguageID = getLanguageID(Languages, temp.language);
                            courseRei.SpecificationID = newSpec.SpecID;
                            courseRei.ParentID = courses[courses.Count - 1].SpecificationItemID;
                            courseRei.Value = m.content;
                            courseRei.OrderNumber = subItemCounter++;
                            courses.Add(courseRei);
                        }
                    }
                }



                str = "\n";
                str += newSpec.Code + " : "
                    + newSpec.Class + " : "
                    + newSpec.RotationID + " : "
                    + newSpec.MealTypeID + " : "
                    + newSpec.StatusID + " : "
                    + newSpec.Station;

                Console.WriteLine(str);


                foreach (SpecificationItemTransferObject m in courses)
                {
                    str = "";
                    str += m.OrderNumber + ":"
                        + m.ParentID + ":"
                        + m.SpecificationID + ":"
                        + m.SpecificationItemID + ":  ";
                    if (m.ParentID != 0)
                        str += " ";
                    str += m.Value;
                    Console.WriteLine(str);
                }
                Console.WriteLine("enter?");
                Console.ReadLine();


                if (newSpec.SpecID > 0)
                {
                    // ----------- Process the Spec Items .......

                    httpResponse = client.PostAsJsonAsync<List<SpecificationItemTransferObject>>(ajaxURI + "/ProcessSpecificationItems", courses).Result;
                    Console.WriteLine("\n|--------------------|");
                    Console.WriteLine("... Attaching SpecItems for new Spec");
                    Console.WriteLine("Data Sent.. did it work? ....");

                    if (httpResponse.StatusCode == (HttpStatusCode)200)
                    {
                        Console.WriteLine("yes....yes it did");
                        SpecificationItemResponseObject PressXtoJSON = new SpecificationItemResponseObject();
                        try
                        {
                            PressXtoJSON = await httpResponse.Content.ReadAsAsync<SpecificationItemResponseObject>();
                            //if (PressXtoJSON.Error)
                            //{
                            //    Console.WriteLine("the faiL, has arrived :(");
                            //    foreach (Content_Centre_API_CORS.ErrorHandling.SpecificationError m in PressXtoJSON.ErrorList)
                            //    {
                            //        Console.WriteLine("-> " + m.ErrorMessage);
                            //    }
                            //}
                            //else
                            //{
                            //    Console.WriteLine("Spec was a GREAT SUCCESS");
                            //    newSpec.SpecID = PressXtoJSON.SpecID;
                            //}
                        }
                        catch (Newtonsoft.Json.JsonSerializationException e)
                        {
                            Console.WriteLine("the faiL, has arrived :(");
                            Console.WriteLine("-> " + e.Message);
                        }
                        //SpecRes = JsonConvert.DeserializeObject<List<SpecTransferObject>>(PressXtoJSON.JsonResults);

                    }
                    else
                    {
                        Console.WriteLine("nope, wrong again!");
                        Console.WriteLine("--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase);
                    }
                }
                else
                {
                    Console.WriteLine("can't add specs without ID");
                }
            }
            Console.WriteLine("one more ENTER");
            Console.ReadLine();


        }

        public int getAllergenID(List<AllergenTransferObject> Languages, String finder)
        {
            AllergenTransferObject findThis = new AllergenTransferObject();
            finder = finder.ToLower();
            findThis.Value = finder;

            for (int m = 0; m < Languages.Count; m++)
            {
                if (Languages[m].Value.ToLower() == findThis.Value)
                {
                    findThis.AllergenID = Languages[m].AllergenID;
                }
            }
            return findThis.AllergenID;
        }

        public int getLanguageID(List<LanguageTransferObject> Languages, String finder)
        {
            LanguageTransferObject findThis = new LanguageTransferObject();
            finder = finder.ToLower();
            findThis.LanguageCode = finder;

            for (int m = 0; m < Languages.Count; m++)
            {
                if (Languages[m].LanguageCode.ToLower() == findThis.LanguageCode)
                {
                    findThis.LanguageID = Languages[m].LanguageID;
                }
            }
            return findThis.LanguageID;
        }

        public int getMealTypeID(List<MealTypeTransferObject> MealTypes, String finder)
        {
            MealTypeTransferObject findThis = new MealTypeTransferObject();
            finder = finder.ToLower();
            findThis.MealType = finder; //"CSML";

            for (int m = 0; m < MealTypes.Count; m++)
            {
                if (MealTypes[m].MealType.ToLower() == findThis.MealType)
                {
                    findThis.MealTypeID = MealTypes[m].MealTypeID;
                }
            }
            return findThis.MealTypeID;
        }

        public int getRotationGrillID(List<RotationGrillTransferObject> Rotations, String finder)
        {
            RotationGrillTransferObject findThis = new RotationGrillTransferObject();
            finder = finder.ToLower();
            findThis.RotationName = finder;

            for (int m = 0; m < Rotations.Count; m++)
            {
                if (Rotations[m].RotationName.ToLower() == findThis.RotationName)
                {
                    findThis.RotationID = Rotations[m].RotationID;
                }
            }
            return findThis.RotationID;
        }

        public int getStatusID(List<StatusTransferObject> Statuses, String finder)
        {
            StatusTransferObject findThis = new StatusTransferObject();
            finder = finder.ToLower();
            if (finder == "")
                finder = "in progress";
            if (finder == "brouillon") // draft?
                finder = "in progress";

            findThis.Status = finder;

            for (int m = 0; m < Statuses.Count; m++)
            {
                if (Statuses[m].Status.ToLower() == findThis.Status)
                {
                    findThis.StatusID = Statuses[m].StatusID;
                }
            }
            return findThis.StatusID;
        }

        protected override void OnStart(string[] args)
        {


        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }

        public async Task<String> GetAuthToken()
        {

            String accessToken = "";

            try
            {
                CookieContainer cookies = new CookieContainer();
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = cookies;

                HttpClient client = new HttpClient(handler);
                Uri uri = new Uri("https://oscid.osc-it.com/account/signin");
                HttpResponseMessage response = client.GetAsync(uri).Result;

                string content = await response.Content.ReadAsStringAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(content);
                string tokenValue = "";
                var reqVerTokenElement = htmlDoc
                                    .DocumentNode
                                    .Descendants("input")
                                    .Where(n => n.Attributes["name"] != null
                                                && n.Attributes["name"].Value
                                                    == "__RequestVerificationToken")
                                    .FirstOrDefault();

                if (reqVerTokenElement != null)
                {
                    tokenValue = reqVerTokenElement.Attributes["value"].Value;
                }

                string postUrl = "https://oscid.osc-it.com/account/signin";

                StringContent postContent = new StringContent("__RequestVerificationToken=" + tokenValue + "&UserName=frankAF&Password=asdqwe123&EnableSSO=false");
                postContent.Headers.ContentType.MediaType = "application/x-www-form-urlencoded";
                HttpResponseMessage newResponse = client.PostAsync(new Uri(postUrl), postContent).Result;

                //String jwtURL = "https://oscid.osc-it.com/issue/oauth2/authorize?client_id=tt_ContentCentreProdInternal&scope=urn%3AContentCentreProdInternal&response_type=token&redirect_uri=https%3A%2F%2Fcontentcentre.oscww.com%2F";
                String jwtURL = "https://oscid.osc-it.com/issue/oauth2/authorize?client_id=tt_ContentCentreWeb&scope=urn%3AContentCentreWeb&response_type=token&redirect_uri=https%3A%2F%2Flocalhost%2FContentCentreWeb%2Findex.html";
                response = client.GetAsync(jwtURL).Result;
                accessToken = response.RequestMessage.RequestUri.Fragment.Substring(0, response.RequestMessage.RequestUri.Fragment.IndexOf("&"));

            }
            catch (Exception ex)
            {
            }

            return accessToken;

        }
    }
}
