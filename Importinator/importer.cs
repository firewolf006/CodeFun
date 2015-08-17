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
using Content_Centre_API.Messaging;
using System.Net;
using System.Net.Http;
using HtmlAgilityPack;
using Newtonsoft.Json;
//using ProjectManagement_API.Models.DTO;
//using Content_Centre_API.Messaging;
using Content_Centre_API_CORS.Messaging;
//using System.Globalization;


// C:\Users\LesterS\Downloads\JsonOutput2014-12-10-10-685.txt

// c:\parseMe3-2.txt



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


            // get the specs to be imported
            String lineS = "";
            StreamReader reader = File.OpenText(args[0]);
            lineS = reader.ReadToEnd();

            List<String> errors = new List<String>();
            String error;

            theList test = new theList();
            test = JsonConvert.DeserializeObject<theList>(lineS);

            bool VERBOSE = false;



            foreach (DataList m in test.menuDataList)
            {
                // build code string
                for (int x = 0; x < m.mealList.Count; x++)
                {
                    m.newCode += m.mealList[x].code;
                    if (x + 1 != m.mealList.Count)
                        m.newCode += "/";
                }

                // build class string
                for (int x = 0; x < m.classCodeList.Count; x++)
                {
                    m.newClass += m.classCodeList[x].code;
                    if (x + 1 != m.classCodeList.Count)
                        m.newClass += "/";
                }

                // build station string
                for (int x = 0; x < m.departureArrivalList.Count; x++)
                {
                    m.newStation += m.departureArrivalList[x].departure.code;
                    if (m.departureArrivalList[x].arrival != null)
                        m.newStation += "(" + m.departureArrivalList[x].arrival.code + ")";
                    if (x + 1 != m.departureArrivalList.Count)
                        m.newStation += "/";
                }

                m.specMatchKey = m.newCode +
                    m.validityBegin + m.pairingGrid + m.cycle + m.newStation +
                    m.newClass;

                if (m.OSCid > -1)
                    m.tempSpecID = m.OSCid;
            }

            //assign temporary specIDs
            int tempSpecCounter = -2;

            for (int y = 0; y < test.menuDataList.Count(); y++)
            {
                if (test.menuDataList[y].tempSpecID == -1)
                {
                    test.menuDataList[y].tempSpecID = tempSpecCounter;

                    bool found = false;
                    for (int x3 = y + 1; x3 < test.menuDataList.Count(); x3++)
                    {
                        //test.menuDataList[x3].tempSpecID = --tempSpecCounter;

                        if (test.menuDataList[x3].tempSpecID == -1)
                        {
                            if (test.menuDataList[y].specMatchKey == test.menuDataList[x3].specMatchKey)
                            {
                                test.menuDataList[x3].tempSpecID = test.menuDataList[y].tempSpecID;
                                found = true;
                            }
                        }
                    }
                    if (found)
                        tempSpecCounter--;
                }
            }

            foreach (DataList m in test.menuDataList)
            {
                Console.WriteLine(m.specMatchKey + " : " + m.tempSpecID);
            }


            // get Token
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            Console.WriteLine("Getting token");

            HttpClient client = new HttpClient();
                string OscToken = string.Empty;
            // Uncomment this to use a fresh token
            //OscToken = "Bearer " + await GetAuthToken();
            //OscToken = OscToken.Replace("#access_token=", "");
            
            // FrankAF -----
            //OscToken = "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczovL29zY2lkLm9zYy1pdC5jb20vdHJ1c3QvSWRlbnRpdHlTZXJ2ZXIiLCJhdWQiOiJ1cm46Q29udGVudENlbnRyZVdlYiIsIm5iZiI6MTQzOTQ5NDc5NCwiZXhwIjoxNDQwNzA0Mzk0LCJuYW1laWQiOiJmcmFua0FGIiwidW5pcXVlX25hbWUiOiJmcmFua0FGIiwiYXV0aG1ldGhvZCI6Imh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9hdXRoZW50aWNhdGlvbm1ldGhvZC9wYXNzd29yZCIsImF1dGhfdGltZSI6IjIwMTUtMDgtMTNUMTk6Mzk6NTMuNzA4WiIsImVtYWlsIjoiZnJhbmtBRkBhaXJsaW5lbWVudXMuY29tIiwicm9sZSI6WyJBaXIgRnJhbmNlIC0gRm9vZCBEZXNpZ25lciIsIkFpciBGcmFuY2UgLSBJbmZvcm1hdGlvbiBUZWNobm9sb2d5Il0sImh0dHA6Ly9pZGVudGl0eXNlcnZlci50aGlua3RlY3R1cmUuY29tL2NsYWltcy9wcm9maWxlY2xhaW1zL2NvbXBhbnlpZCI6IjEifQ.VPR4WbFPW7t5YFRcAZ-TbmPf9HtvZfr_wNFVwKaNoTM";
            //FrankOSC -----
            OscToken = "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczovL29zY2lkLm9zYy1pdC5jb20vdHJ1c3QvSWRlbnRpdHlTZXJ2ZXIiLCJhdWQiOiJ1cm46Q29udGVudENlbnRyZVdlYiIsIm5iZiI6MTQzOTQ5NTAxOCwiZXhwIjoxNDQwNzA0NjE4LCJuYW1laWQiOiJmcmFua09TQyIsInVuaXF1ZV9uYW1lIjoiZnJhbmtPU0MiLCJhdXRobWV0aG9kIjoiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2F1dGhlbnRpY2F0aW9ubWV0aG9kL3Bhc3N3b3JkIiwiYXV0aF90aW1lIjoiMjAxNS0wOC0xM1QxOTo0MzozOC43NDZaIiwiZW1haWwiOiJmcmFua09TQ0BvbmV3b3JsZG9uZXN0b3AuY29tIiwicm9sZSI6WyJPU0MgLSBBaXIgRnJhbmNlIFRlYW0gTWVtYmVyIiwiT1NDIC0gQnJpdGlzaCBBaXJ3YXlzIFRlYW0gTWVtYmVyIiwiT1NDIC0gSVQgQWRtaW4iLCJPU0MgLSBVbml0ZWQgVGVhbSBNZW1iZXIiLCJPU0MgLSBWaXJnaW4gQXRsYW50aWMgVGVhbSBNZW1iZXIiXSwiaHR0cDovL2lkZW50aXR5c2VydmVyLnRoaW5rdGVjdHVyZS5jb20vY2xhaW1zL3Byb2ZpbGVjbGFpbXMvY29tcGFueWlkIjoiNSJ9.xmwOfF7yn_4S6oXL2wF5m9A246czE2SIN0y4xIF6CRk";
            client.DefaultRequestHeaders.Add("Authorization", OscToken);
            client.DefaultRequestHeaders.Referrer = new System.Uri("http://ifonlythiswasarealsite.com/JSONimporter.txt");
            //client.DefaultRequestHeaders.Add("


            Console.WriteLine("token Retrieved");

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

            //clientDTO testClient = new clientDTO();
            //projectDTO proJ = new projectDTO();
            //responseObject resultObject = new responseObject();
            //ProjectManagementRequest reQ = new ProjectManagementRequest();

            //testClient.name = "Paul Thorott";
            //testClient.email = "Cloud.Strife@ffxiv.pol";
            //testClient.phoneNumber = "1.123.123.1234";

            //Console.WriteLine("some text....");

            //proJ.client = testClient;
            //proJ.documents = new List<documentDTO>();
            //proJ.resources = new List<resourceDTO>();
            //proJ.invoices = new List<invoiceDTO>();
            //proJ.projectName = "What What?";

            //documentDTO tempDoc = new documentDTO();
            //resourceDTO tempRes = new resourceDTO();
            //invoiceDTO tempInv = new invoiceDTO();
            //languageDTO tempLan = new languageDTO();

            //tempLan.translateDirectionID = new List<int>();
            //tempLan.translateDirectionID.Add(3);
            //tempLan.translateDirectionID.Add(2);            

            //tempDoc.name = "DOC1";
            //tempDoc.filePath = "x:\\sick.jpg";
            //tempDoc.status = 1;
            //tempDoc.sourceLanguageID = 1;
            //tempDoc.translateDirections = new languageDTO();
            //tempDoc.translateDirections = tempLan;


            //tempDoc.typeID = 1;

            //tempRes.firstName = "Don";
            //tempRes.lastName = "Matroska";
            //tempRes.company = "OSC";
            //tempRes.address = "100 disney drive";
            //tempRes.type = "0";
            //tempRes.isCompany = false;
            //tempRes.languageAssociations = new languageDTO();
            //tempRes.languageAssociations = tempLan;
            //tempRes.languageAssociations.translateDirectionID[0] = 1;
            //tempRes.languageAssociations.translateDirectionID[1] = 3;
            //tempRes.documents = new List<documentDTO>();

            //tempInv.price = 0.50;
            //tempInv.status = "1";
            //tempInv.version = 1.00;
            //tempInv.documentID = 344;

            //proJ.invoices.Add(tempInv);
            //proJ.documents.Add(tempDoc);
            //proJ.resources.Add(tempRes);


            //reQ.projects = new List<projectDTO>();

            //reQ.projects.Add(proJ);

            Console.WriteLine("some more text....");

            //String ajaxURI = "https://localhost/ProjectManagement-API/api/projectmanagement/postProject/";



            //HttpResponseMessage httpResponse = client.PostAsJsonAsync<ProjectManagementRequest>(ajaxURI, reQ).Result;
            //resultObject = httpResponse.Content.ReadAsAsync<responseObject>().Result;

            //MealTypeResponseObject something = new MealTypeResponseObject();

            //List<MealTypeTransferObject> MealTypes = new List<MealTypeTransferObject>();

            // this is where the magic happens.....
            List<MealTypeTransferObject> MealTypes = getIDs<MealTypeTransferObject>(client, "getAllMealTypes", "Meal Types", errors).Result;
            List<StatusTransferObject> SpecStatuses = getIDs<StatusTransferObject>(client, "getAllStatuses", "Status", errors).Result;
            List<RotationGrillTransferObject> Rotations = getIDs<RotationGrillTransferObject>(client, "getRotationByName", "Rotation Grill", errors).Result;
            List<LanguageTransferObject> Languages = getIDs<LanguageTransferObject>(client, "getAllLanguages", "Languages", errors).Result;
            List<AllergenTransferObject> Allergens = getIDs<AllergenTransferObject>(client, "getAllAllergens", "Allergens", errors).Result;


            String ajaxURI = ConfigurationManager.AppSettings["SpecWebApi"];

            //// -----------Get meal types
            //MealTypeTransferObject reQ1 = new MealTypeTransferObject();

            HttpResponseMessage httpResponse;
            //HttpResponseMessage httpResponse = client.PostAsJsonAsync<MealTypeTransferObject>(ajaxURI + "/getAllMealTypes", reQ1).Result;
            //Console.WriteLine("\n|--------------------|");
            //Console.WriteLine("... Getting Meal Type IDs");
            //Console.WriteLine("Data Sent.. did it work? ....");

            //if (httpResponse.StatusCode == (HttpStatusCode)200)
            //{
            //    //Console.WriteLine("yes....yes it did");
            //    MealTypeResponseObject PressXtoJSON = new MealTypeResponseObject();
            //    PressXtoJSON = await httpResponse.Content.ReadAsAsync<MealTypeResponseObject>();
            //    MealTypes = JsonConvert.DeserializeObject<List<MealTypeTransferObject>>(PressXtoJSON.JsonResults);
            //    Console.WriteLine("MealTypes populated");
            //}
            //else
            //{
            //    Console.WriteLine("nope, wrong again!");
            //    error = "--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase;
            //    errors.Add("MealType: " + error);
            //    Console.WriteLine(error);
            //}



            //// ---------Get Status Codes
            //StatusTransferObject reQ2 = new StatusTransferObject();
            //List<StatusTransferObject> SpecStatuses = new List<StatusTransferObject>();

            //HttpResponseMessage httpResponse = client.PostAsJsonAsync<StatusTransferObject>(ajaxURI + "/getAllStatuses", reQ2).Result;
            //Console.WriteLine("\n|--------------------|");
            //Console.WriteLine("... Getting Status IDs");
            //Console.WriteLine("Data Sent.. did it work? ....");

            //if (httpResponse.StatusCode == (HttpStatusCode)200)
            //{
            //    //Console.WriteLine("yes....yes it did");
            //    StatusResponseObject PressXtoJSON = new StatusResponseObject();
            //    PressXtoJSON = await httpResponse.Content.ReadAsAsync<StatusResponseObject>();
            //    SpecStatuses = JsonConvert.DeserializeObject<List<StatusTransferObject>>(PressXtoJSON.JsonResults);
            //    Console.WriteLine("Statuses populated");
            //}
            //else
            //{
            //    Console.WriteLine("nope, wrong again!");
            //    Console.WriteLine("--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase);
            //}

            //// -----------Get rotations
            //RotationGrillTransferObject reQ3 = new RotationGrillTransferObject();
            //List<RotationGrillTransferObject> Rotations = new List<RotationGrillTransferObject>();

            //httpResponse = client.PostAsJsonAsync<RotationGrillTransferObject>(ajaxURI + "/getRotationByName", reQ3).Result;
            //Console.WriteLine("\n|--------------------|");
            //Console.WriteLine("... Rotation Grill IDs");
            //Console.WriteLine("Data Sent.. did it work? ....");

            //if (httpResponse.StatusCode == (HttpStatusCode)200)
            //{
            //    //Console.WriteLine("yes....yes it did");
            //    RotationGrillResponseObject PressXtoJSON = new RotationGrillResponseObject();
            //    PressXtoJSON = await httpResponse.Content.ReadAsAsync<RotationGrillResponseObject>();
            //    Rotations = JsonConvert.DeserializeObject<List<RotationGrillTransferObject>>(PressXtoJSON.JsonResults);
            //    Console.WriteLine("Rotations populated");
            //}
            //else
            //{
            //    Console.WriteLine("nope, wrong again!");
            //    Console.WriteLine("--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase);
            //}

            //// -----------Get languages
            //LanguageTransferObject reQ4 = new LanguageTransferObject();
            //List<LanguageTransferObject> Languages = new List<LanguageTransferObject>();

            //httpResponse = client.PostAsJsonAsync<LanguageTransferObject>(ajaxURI + "/getAllLanguages", reQ4).Result;
            //Console.WriteLine("\n|--------------------|");
            //Console.WriteLine("... Rotation Grill IDs");
            //Console.WriteLine("Data Sent.. did it work? ....");

            //if (httpResponse.StatusCode == (HttpStatusCode)200)
            //{
            //    //Console.WriteLine("yes....yes it did");
            //    LanguageResponseObject PressXtoJSON = new LanguageResponseObject();
            //    PressXtoJSON = await httpResponse.Content.ReadAsAsync<LanguageResponseObject>();
            //    Languages = JsonConvert.DeserializeObject<List<LanguageTransferObject>>(PressXtoJSON.JsonResults);
            //    Console.WriteLine("Languages populated");
            //}
            //else
            //{
            //    Console.WriteLine("nope, wrong again!");
            //    Console.WriteLine("--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase);
            //}

            //// -----------Get allergens....
            //AllergenTransferObject reQ5 = new AllergenTransferObject();
            //reQ5.AccountID = 1;
            ////reQ5.LanguageID = 1;

            //List<AllergenTransferObject> Allergens = new List<AllergenTransferObject>();

            //httpResponse = client.PostAsJsonAsync<AllergenTransferObject>(ajaxURI + "/getAllAllergens", reQ5).Result;
            //Console.WriteLine("\n|--------------------|");
            //Console.WriteLine("... getting Allergen IDs");
            //Console.WriteLine("Data Sent.. did it work? ....");
            ////Console.WriteLine("NOPE...... just kidding");

            //if (httpResponse.StatusCode == (HttpStatusCode)200)
            //{
            //    //Console.WriteLine("yes....yes it did");
            //    AllergenResponseObject PressXtoJSON = new AllergenResponseObject();
            //    PressXtoJSON = await httpResponse.Content.ReadAsAsync<AllergenResponseObject>();
            //    Allergens = JsonConvert.DeserializeObject<List<AllergenTransferObject>>(PressXtoJSON.JsonResults);
            //    Console.WriteLine("Allergens populated");
            //}
            //else
            //{
            //    Console.WriteLine("nope, wrong again!");
            //    Console.WriteLine("--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase);
            //}

            //List<AllergenTransferObject> Allergens = new List<AllergenTransferObject>();
            //AllergenTransferObject tempAlly = new AllergenTransferObject();

            //tempAlly.AllergenID = 0;
            //tempAlly.Value = "Fish";
            //Allergens.Add(tempAlly);

            //tempAlly.AllergenID = 1;
            //tempAlly.Value = "Egg";
            //Allergens.Add(tempAlly);

            //tempAlly.AllergenID = 2;
            //tempAlly.Value = "Gluten";
            //Allergens.Add(tempAlly);

            //tempAlly.AllergenID = 3;
            //tempAlly.Value = "Bees";
            //Allergens.Add(tempAlly);

            //tempAlly.AllergenID = 4;
            //tempAlly.Value = "Milk";
            //Allergens.Add(tempAlly);

            // ------------------------////////////////////////

            //COURSE pole = new COURSE();
            //List<ALLERGEN> oool = new List<ALLERGEN>();
            //ALLERGEN looo = new ALLERGEN();


            //looo.allergen = "milk";
            //oool.Add(looo);

            //looo.allergen = "Egg";
            //oool.Add(looo);

            //looo.allergen = "BeeS";
            //oool.Add(looo);

            //pole.allergens = oool;

            //String PressMtoJSON = "";
            ////PressMtoJSON = JsonConvert.DeserializeObject<theList>(lineS);
            //PressMtoJSON = JsonConvert.SerializeObject(pole);

            bool loadError = false;
            if (MealTypes.Count == 0)
                loadError = true;
            if (SpecStatuses.Count == 0)
                loadError = true;
            if (Rotations.Count == 0)
                loadError = true;
            if (Languages.Count == 0)
                loadError = true;
            if (Allergens.Count == 0)
                loadError = true;


            // display the specs for verification

            if (!loadError)
            {

                String str = "";
                DateTime startTime = new DateTime();
                TimeSpan diffTime = new TimeSpan();
                startTime = DateTime.Now;
                int start = 0, howMany = 100; //9
                if (howMany + start > test.menuDataList.Count)
                    howMany = test.menuDataList.Count - start ;
                for (int xList = start; xList < howMany + start; xList++)
                {
                    DataList temp = new DataList();
                    SpecTransferObject newSpec = new SpecTransferObject();
                    Content_Centre_API_CORS.Templates.AccountContext GlobalContext = new Content_Centre_API_CORS.Templates.AccountContext();
                    GlobalContext.AccountID = 1;
                    GlobalContext.Role = "OSC - Air France Team Member";

                    newSpec.Context = GlobalContext;

                    DateTime nowTime = new DateTime();

                    nowTime = DateTime.Now;
                    diffTime = nowTime - startTime;

                    String strTemp = "";
                    nowTime  = startTime + TimeSpan.FromMilliseconds((diffTime.TotalMilliseconds / (xList + 1)) * howMany);


                    strTemp += "Start:" + startTime.ToShortTimeString() +
                         " " + (xList + 1) + "\\" + howMany + "(" + String.Format("{0:p}",((xList + 1.0) / howMany)) + ") done at " +
                         nowTime.ToShortTimeString();
                    Console.Clear();
                    Console.WriteLine(strTemp);

                    bool IDError = false;

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

                    newSpec.Class = temp.newClass;
                    newSpec.Code = temp.newCode;
                    newSpec.Station = temp.newStation;


                    // get MealType
                    newSpec.MealTypeID = getMealTypeID(MealTypes, temp.mealType);
                    if (newSpec.MealTypeID == -1) //new mealType?
                    {
                        IDError = true;
                        error = "-> mealType \"" + temp.mealType + "\" not found";
                        errors.Add(newSpec.SpecID + ":" + temp.specMatchKey + ">>\n" + error);
                        Console.WriteLine(error);
                    }

                    // get Status Code
                    newSpec.StatusID = getStatusID(SpecStatuses, temp.state);
                    if (newSpec.StatusID == -1) //new Status?
                    {
                        IDError = true;
                        error = "-> Status \"" + temp.state + "\" not found";
                        errors.Add(newSpec.SpecID + ":" + temp.specMatchKey + ">>\n" + error);
                        Console.WriteLine(error);
                    }

                    // Get Rotation ID?
                    newSpec.RotationID = getRotationGrillID(Rotations, temp.pairingGrid);
                    if (temp.pairingGrid == "X")
                    {
                        newSpec.RotationID = -1;
                    }
                    else if (newSpec.RotationID == -1) //new Rotation?
                    {
                        IDError = true;
                        error = "-> Rotation \"" + temp.pairingGrid + "\" not found";
                        errors.Add(newSpec.SpecID + ":" + temp.specMatchKey + ">>\n" + error);
                        Console.WriteLine(error);
                    }

                    // ----------- insert NEW and Get Spec ID .......

                    //SpecTransferObject reQ5 = new SpecTransferObject();
                    //List<LanguageTransferObject> Languages = new List<LanguageTransferObject>();




                    if (!IDError)
                    {


                        if (temp.tempSpecID > -1) // check if spec exists
                        {
                            SpecTransferObject checkSpec = new SpecTransferObject();
                            checkSpec.SpecID = temp.tempSpecID;
                            checkSpec.Context = GlobalContext;


                            httpResponse = client.PostAsJsonAsync<SpecTransferObject>(ajaxURI + "/getSpecByID", checkSpec).Result;
                            if (VERBOSE)
                            {
                                Console.WriteLine("\n|--------------------|");
                                Console.WriteLine("... Checking if Spec Exists");
                                Console.WriteLine("Data Sent.. did it work? ....");
                            }

                            if (httpResponse.StatusCode == (HttpStatusCode)200)
                            {
                                //Console.WriteLine("yes....yes it did");
                                SpecResponseObject PressXtoJSON = new SpecResponseObject();
                                try
                                {
                                    PressXtoJSON = await httpResponse.Content.ReadAsAsync<SpecResponseObject>();
                                    if (PressXtoJSON.Error)
                                    {
                                        Console.WriteLine("the faiL, has arrived :(");
                                        foreach (Content_Centre_API_CORS.ErrorHandling.SpecificationError m in PressXtoJSON.ErrorList)
                                        {
                                            error = "-> " + m.ErrorMessage;
                                            errors.Add("Check Spec " + error);
                                            Console.WriteLine(error);


                                        }
                                    }
                                    else
                                    {
                                        if (VERBOSE)
                                        {
                                            Console.WriteLine("SpecCheck was a GREAT SUCCESS");
                                        }
                                        DataTable tempResponse = new DataTable();
                                        tempResponse = JsonConvert.DeserializeObject<DataTable>(PressXtoJSON.JsonResults);
                                        try
                                        {
                                            newSpec.SpecID = Convert.ToInt32(tempResponse.Rows[0].ItemArray[0]);
                                        }

                                        catch (SystemException e)
                                        {
                                            error = e.Message.ToString();
                                            errors.Add("Get SpecID " + error);
                                            Console.WriteLine(error);


                                        }
                                        if (newSpec.SpecID < 0)
                                        {
                                            Console.WriteLine("no, no, no.... it's not there... can't update");
                                            errors.Add("Can't Update Spec " + checkSpec.SpecID + ":  " + temp.specMatchKey);
                                            newSpec.SpecID = 0;
                                        }
                                        // check if there are matching spec and assign the new spec ID
                                    }
                                }
                                catch (Newtonsoft.Json.JsonSerializationException e)
                                {
                                    Console.WriteLine("the faiL, has arrived :(");
                                    error = e.Message.ToString();
                                    errors.Add("JSON? " + error);
                                    Console.WriteLine(error);
                                }
                                //SpecRes = JsonConvert.DeserializeObject<List<SpecTransferObject>>(PressXtoJSON.JsonResults);

                            }
                            else
                            {
                                Console.WriteLine("nope, wrong again!");
                                //Console.WriteLine("--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase);
                                error = "--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase;
                                errors.Add("Get Spec ID (API Call) " + error);
                                Console.WriteLine(error);
                            }
                        }

                        if (temp.tempSpecID < 0) // INSERT
                        {

                            httpResponse = client.PostAsJsonAsync<SpecTransferObject>(ajaxURI + "/insertSpec", newSpec).Result;
                            if (VERBOSE)
                            {
                                Console.WriteLine("\n|--------------------|");
                                Console.WriteLine("... Getting Spec ID for new Spec");
                                Console.WriteLine("Data Sent.. did it work? ....");
                            }

                            if (httpResponse.StatusCode == (HttpStatusCode)200)
                            {
                                //Console.WriteLine("yes....yes it did");
                                SpecResponseObject PressXtoJSON = new SpecResponseObject();
                                try
                                {
                                    PressXtoJSON = await httpResponse.Content.ReadAsAsync<SpecResponseObject>();
                                    if (PressXtoJSON.Error)
                                    {
                                        Console.WriteLine("the faiL, has arrived :(");
                                        foreach (Content_Centre_API_CORS.ErrorHandling.SpecificationError m in PressXtoJSON.ErrorList)
                                        {
                                            error = "-> " + m.ErrorMessage;
                                            errors.Add(temp.specMatchKey + " failed on insert " + error);
                                            Console.WriteLine(error);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Spec was a GREAT SUCCESS");
                                        newSpec.SpecID = PressXtoJSON.SpecID;
                                        int toChange = test.menuDataList[xList].tempSpecID;
                                        test.menuDataList[xList].tempSpecID = PressXtoJSON.SpecID;
                                        // check if there are matching spec and assign the new spec ID



                                        for (int xUpdate = xList + 1; xUpdate < howMany + start; xUpdate++)
                                        {
                                            if (test.menuDataList[xUpdate].tempSpecID == toChange)
                                                test.menuDataList[xUpdate].tempSpecID = newSpec.SpecID;
                                        }
                                    }
                                }
                                catch (Newtonsoft.Json.JsonSerializationException e)
                                {
                                    Console.WriteLine("the faiL, has arrived :(");
                                    error = "-> " + e.Message;
                                    errors.Add("JSon!!!! " + error);
                                    Console.WriteLine(error);
                                }
                                //SpecRes = JsonConvert.DeserializeObject<List<SpecTransferObject>>(PressXtoJSON.JsonResults);

                            }
                            else
                            {
                                Console.WriteLine("nope, wrong again!");
                                error = "--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase;
                                errors.Add("Insert Spec ID (API Call) " + error);
                                Console.WriteLine(error);
                            }
                        }
                        else if (temp.tempSpecID != 0)
                        { // UPDATE!!!!
                            httpResponse = client.PostAsJsonAsync<SpecTransferObject>(ajaxURI + "/updateSpec", newSpec).Result;
                            if (VERBOSE)
                            {
                                Console.WriteLine("\n|--------------------|");
                                Console.WriteLine("... Getting Spec ID for new Spec");
                                Console.WriteLine("Data Sent.. did it work? ....");
                            }

                            if (httpResponse.StatusCode == (HttpStatusCode)200)
                            {
                                //Console.WriteLine("yes....yes it did");
                                SpecResponseObject PressXtoJSON = new SpecResponseObject();
                                try
                                {
                                    PressXtoJSON = await httpResponse.Content.ReadAsAsync<SpecResponseObject>();
                                    if (PressXtoJSON.Error)
                                    {
                                        Console.WriteLine("the faiL, has arrived :(");
                                        foreach (Content_Centre_API_CORS.ErrorHandling.SpecificationError m in PressXtoJSON.ErrorList)
                                        {
                                            error = "-> " + m.ErrorMessage;
                                            errors.Add("update error " + error);
                                            Console.WriteLine(error);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Spec was a GREAT SUCCESS");
                                    }
                                }
                                catch (Newtonsoft.Json.JsonSerializationException e)
                                {
                                    Console.WriteLine("the faiL, has arrived :(");
                                    error = "-> " + e.Message;
                                    errors.Add("JSON!!! " + error);
                                    Console.WriteLine(error);
                                }
                                //SpecRes = JsonConvert.DeserializeObject<List<SpecTransferObject>>(PressXtoJSON.JsonResults);

                            }
                            else
                            {
                                Console.WriteLine("nope, wrong again!");
                                error = "--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase;
                                errors.Add("Update Spec (API Call) " + error);
                                Console.WriteLine(error);
                            }
                        }
                        else
                        {
                            error = "Spec ID is invalid, i don't know what to do";
                            errors.Add(newSpec.SpecID + ": " + error);
                            Console.WriteLine(error);
                        }

                        str = "\n";
                        str += newSpec.Code + " : "
                            + newSpec.Class + " : "
                            + newSpec.RotationID + " : "
                            + newSpec.MealTypeID + " : "
                            + newSpec.StatusID + " : "
                            + newSpec.Station;

                        Console.WriteLine(str);

                        bool abortTheMission = false; // if there're any errors in the spec Items

                        if (newSpec.SpecID > 0)
                        {

                            List<SpecificationItemTransferObject> courses = new List<SpecificationItemTransferObject>();

                            int headerCounter = 1, subItemCounter = 1;

                            for (int x = 0; x < temp.menusList[0].coursesList.Count; x++) // <--- why zero?
                            {

                                COURSE m = temp.menusList[0].coursesList[x];
                                SpecificationItemTransferObject tempCourse = new SpecificationItemTransferObject();
                                tempCourse.AccountID = "1"; // Air France
                                int xID = x + 1;

                                tempCourse.SpecificationItemID = -(courses.Count + 1);
                                tempCourse.LanguageID = getLanguageID(Languages, temp.language);
                                if (tempCourse.LanguageID == -1) //new language?
                                {
                                    abortTheMission = true;
                                    error = "-> language \"" + temp.language + "\" not found";
                                    errors.Add(newSpec.SpecID + ":" + temp.specMatchKey + ">>\n" + error);
                                    Console.WriteLine(error);
                                }
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

                                // get Allergen IDs and apply to specItem
                                foreach (ALLERGEN m2 in m.allergens)
                                {
                                    AllergenTransferObject tempAlly = new AllergenTransferObject();

                                    tempAlly.AccountID = Convert.ToInt32(tempCourse.AccountID);
                                    tempAlly.LanguageID = tempCourse.LanguageID;
                                    tempAlly.AllergenID = getAllergenID(Allergens, m2.allergen, tempAlly.LanguageID);
                                    if (tempAlly.AllergenID == -1) //new allergen?
                                    {
                                        abortTheMission = true;
                                        error = "-> allergen \"" + m2.allergen + "\" not found";
                                        errors.Add(newSpec.SpecID + ":" + tempCourse.Value + " " + error + " for " + temp.language);
                                        Console.WriteLine(error);
                                    }
                                    tempCourse.Allergens.Add(tempAlly);
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

                                        // if i used Linq...
                                        //int x2222 = Languages.Where(lester => lester.LanguageCode == temp.language).Select(y => y.LanguageID).Single();

                                        courseRei.SpecificationID = newSpec.SpecID;
                                        courseRei.ParentID = courses[courses.Count - 1].SpecificationItemID;
                                        courseRei.Value = m.content;
                                        courseRei.OrderNumber = subItemCounter++;

                                        // move allergens from the header
                                        courseRei.Allergens = tempCourse.Allergens;
                                        tempCourse.Allergens = new List<AllergenTransferObject>();
                                        courses.Add(courseRei);
                                    }
                                }
                            }

                            if (abortTheMission)
                            {
                                errors.Add("Spec Item Error/s, " + newSpec.SpecID + ": cannot attach specItems for this spec, one more more allergens are invalid");
                                Console.WriteLine(errors[errors.Count - 1]);
                            }


                            if (VERBOSE)
                            {

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



                                Console.WriteLine("enter? the attachments have beeeguuuun");
                                Console.ReadLine();
                            }


                            if (!abortTheMission)
                            {


                                // ----------- Process the Spec Items .......
                                SpecificationItemTransferObjects SpecItemDTO = new SpecificationItemTransferObjects();
                                SpecificationItemTransferObject tempSpecGet = new SpecificationItemTransferObject();
                                //SpecItemDTO.data = courses;

                                //SpecItemDTO

                                tempSpecGet.SpecificationID = newSpec.SpecID;
                                SpecItemDTO.Context = GlobalContext;
                                tempSpecGet.Context = GlobalContext;
                                //SpecItemDTO.data[0].SpecificationID = newSpec.SpecID;

                                //DataTable tempResponse = new DataTable();
                                //SpecificationItemResponseObject SpecTemp = new SpecificationItemResponseObject();
                                //SpecTemp = await httpResponse.Content.ReadAsAsync<SpecificationItemResponseObject>();

                                //SpecItemDTO = JsonConvert.DeserializeObject<SpecificationItemTransferObjects>(SpecTemp.JsonResults);
                                // nuke old all specItems
                                // will take longer but checking, updating and deleting would be better, but slower for

                                //get old Specs
                                httpResponse = client.PostAsJsonAsync<SpecificationItemTransferObject>(ajaxURI + "/getSpecificationItemsBySpecID", tempSpecGet).Result;
                                SpecificationItemResponseObject toJSON = new SpecificationItemResponseObject();
                                List<SpecificationItemTransferObject> CorSair = new List<SpecificationItemTransferObject>();
                                toJSON = await httpResponse.Content.ReadAsAsync<SpecificationItemResponseObject>();
                                CorSair = JsonConvert.DeserializeObject<List<SpecificationItemTransferObject>>(toJSON.JsonResults);


                                // mark all as toDelete
                                foreach (SpecificationItemTransferObject m in CorSair)
                                {
                                    if (m.LanguageID == courses[0].LanguageID)
                                        m.ToBeDeleted = true;
                                }


                                // append to courses
                                courses.AddRange(CorSair);
                                SpecItemDTO.data = courses;



                                httpResponse = client.PostAsJsonAsync<SpecificationItemTransferObjects>(ajaxURI + "/ProcessSpecificationItems", SpecItemDTO).Result;
                                if (VERBOSE)
                                {

                                    Console.WriteLine("\n|--------------------|");
                                    Console.WriteLine("... Attaching SpecItems for new Spec");
                                    Console.WriteLine("Data Sent.. did it work? ....");
                                }

                                if (httpResponse.StatusCode == (HttpStatusCode)200)
                                {
                                    //Console.WriteLine("yes....yes it did");
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
                                        error = "-> " + e.Message;
                                        errors.Add("JSON!!! " + error);
                                        Console.WriteLine(error);
                                    }
                                    //SpecRes = JsonConvert.DeserializeObject<List<SpecTransferObject>>(PressXtoJSON.JsonResults);

                                }
                                else
                                {
                                    Console.WriteLine("nope, wrong again!");
                                    error = "--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase;
                                    errors.Add("Cant Process Items for " + newSpec.SpecID + ":" + temp.specMatchKey + ": " + error);
                                    Console.WriteLine(error);
                                }
                            }
                        }
                        else
                        {
                            error = "--> can't add specs without ID";
                            errors.Add("Cant Process Items: " + newSpec.SpecID + ":" + temp.specMatchKey + error);
                            Console.WriteLine(error);
                        }
                    }//Console.Clear();
                }
            }
            Console.WriteLine("one more ENTER");
            Console.ReadLine();


            if (errors.Count > 0)
            {

                Console.WriteLine("Yay, errors have occured");
                Console.WriteLine("-----------------------------------\n");
                foreach (string m in errors)
                {
                    Console.WriteLine(m);
                }
                Console.ReadLine();
            }
        }



        /// <summary>
        /// a general function to for getting IDs for objects with varying TransferObjects
        /// </summary>
        /// <typeparam name="T1">transfer object</typeparam>
        /// <typeparam name="T2">response object</typeparam>
        /// <param name="client">HTTP client</param>
        /// <param name="method">method for the api</param>
        /// <param name="moreText">more text for logging</param>
        /// <param name="errors">error List</param>
        public async Task<List<T1>> getIDs<T1>(HttpClient client, string method, string moreText, List<String> errors)
                where T1 : SpecificationRequest, new()
        //where T2, new() // needed one of them here...?? there's no base class
        {
            String ajaxURI = ConfigurationManager.AppSettings["SpecWebApi"];
            string error;

            //var PressXtoJSON = Activator.CreateInstance<T2>();

            // -----------Get meal types
            T1 reQ1 = new T1();
            List<T1> Types = new List<T1>();

            reQ1.Context = new Content_Centre_API_CORS.Templates.AccountContext();
            reQ1.Context.AccountID = 1;
            reQ1.Context.Role = "OSC - Air France Team Member";

            HttpResponseMessage httpResponse = client.PostAsJsonAsync<T1>(ajaxURI + "/" + method, reQ1).Result;
            Console.WriteLine("\n|--------------------|");
            Console.WriteLine("... Getting " + moreText + " IDs");
            Console.WriteLine("Data Sent.. did it work? ....");

            if (httpResponse.StatusCode == (HttpStatusCode)200)
            {
                //Console.WriteLine("yes....yes it did");
                responseObject PressXtoJSON = new responseObject();
                PressXtoJSON = await httpResponse.Content.ReadAsAsync<responseObject>();
                Types = JsonConvert.DeserializeObject<List<T1>>(PressXtoJSON.JsonResults);
                Console.WriteLine(moreText + " populated\n");
                if (Types.Count == 0)
                {
                    errors.Add(moreText + " populated.. but it's empty");
                    Console.WriteLine("....but it's empty");
                }
            }
            else
            {
                Console.WriteLine("nope, wrong again!");
                error = "--> " + httpResponse.StatusCode + " : " + httpResponse.ReasonPhrase;
                errors.Add(moreText + " " + error);
                Console.WriteLine(error);
            }

            return Types;
        }



        // consider combining the next 5 functions
        public int getAllergenID(List<AllergenTransferObject> Allergens, String finder, int LanguageID)
        {
            AllergenTransferObject findThis = new AllergenTransferObject();
            finder = finder.ToLower();
            findThis.Allergen = finder;
            findThis.LanguageID = LanguageID;

            for (int m = 0; m < Allergens.Count; m++)
            {
                if (Allergens[m].Allergen.ToLower() == findThis.Allergen && Allergens[m].LanguageID == findThis.LanguageID)
                {
                    findThis.AllergenID = Allergens[m].AllergenID;
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

                StringContent postContent = new StringContent("__RequestVerificationToken=" + tokenValue + "&UserName=frankOSC&Password=asdqwe123&EnableSSO=false");
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
