/*=====================================================================================================================

[Change History]
[Author]					[Date]				[Notes]
Vladimir Pereira			Jul 27, 2006		Created.
Vladimir Pereira			Jul 28, 2006		Completed UploadDocument method
Vladimir Pereira			Aug 01, 2006		Added Notify method\
Mujahid Kaka                Oct 29, 2008        Added Print, EMAIL and Fax Methods
=====================================================================================================================*/

using System;
using System.Net;
using System.Xml;
using System.Text;
using System.Xml.Linq;
using System.Collections.Generic;



namespace FCT.EPS.WSP.ExternalResources
{
    /// <summary>
    /// SystemServicesWrapper encapsulates the System Services isolating clients
    /// from System Services specific logic
    /// </summary>

    public class SystemServiceWrapper
    {
        //private SystemService.RequestSoapClient _systemService;

        /// <remarks/>
        public SystemServiceWrapper()
        {
            //_systemService = new SystemService.RequestSoapClient();
        }

        /// <summary>
        /// Uploads the documents contained in the documentList structure into DMS system
        /// </summary>
        /// <param name="documentUrl"></param>
        /// <param name="dmsPath"></param>
        /// <param name="categoryName"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public string GenerateDocument(string documentUrl, string dmsPath, string dealId, string userId, string documentDealId, string documentDocId, string documentCustomerId, string policyId = null)
        {
            if (dmsPath == null || dmsPath.Length == 0)
            {
                throw new ArgumentNullException("dmsPath");
            }

            string jobId = null;
            //Prepare XML for the Systems Services request
            ScreenSysServRequestDestinationMethodsSTORE destMethod = new ScreenSysServRequestDestinationMethodsSTORE();
            destMethod.DMSPath = dmsPath;

            ScreenWithCustomerData screen = new ScreenWithCustomerData();

            screen.SysServRequest.Document.SourceURL = documentUrl;

            screen.SysServRequest.Application = "LLC";
            screen.SysServRequest.RequestorID = userId;
            screen.SysServRequest.DealID = dealId;
            screen.SysServRequest.Document.ConvertToPDF = "False";
            screen.SysServRequest.Document.ConvertToText = "False";
            screen.SysServRequest.Document.DocType = "LLC";
            screen.SysServRequest.Document.InfoSource = "NewLLC";
            screen.SysServRequest.Document.ModelDocument = "LLCMaster.CMS";
            ScreenSysServRequestDocumentDocumentDataDocumentID dataDocumentId = new ScreenSysServRequestDocumentDocumentDataDocumentID();

            dataDocumentId.DataType = "Numeric";
            dataDocumentId.Value = documentDocId;
            screen.SysServRequest.Document.DocumentData.DocumentId = dataDocumentId;
            ScreenSysServRequestDocumentDocumentDataDealId dataDealId = new ScreenSysServRequestDocumentDocumentDataDealId();
            dataDealId.DataType = "Numeric";
            dataDealId.Value = documentDealId;
            screen.SysServRequest.Document.DocumentData.DealId = dataDealId;
            ScreenSysServRequestDocumentDocumentDataCustomerId dataCustomerId = new ScreenSysServRequestDocumentDocumentDataCustomerId();
            dataCustomerId.DataType = "Numeric";
            dataCustomerId.Value = documentCustomerId;
            screen.SysServRequest.Document.DocumentData.CustomerId = dataCustomerId;

            ScreenSysServRequestDocumentDocumentDataPolicyId dataPolicyId = new ScreenSysServRequestDocumentDocumentDataPolicyId();
            dataPolicyId.DataType = "Numeric";
            dataPolicyId.Value = policyId ?? "0";
            screen.SysServRequest.Document.DocumentData.PolicyId = dataPolicyId;

            screen.SysServRequest.Destination.Methods.STORE = destMethod;

            // Serialize the request object
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(ScreenWithCustomerData));
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            xmlSerializer.Serialize(stringWriter, screen);

            string requestXmlData = stringWriter.ToString();

            // Submit request
            string responseXmlData = String.Empty;
            using (SystemService.RequestSoapClient client = new SystemService.RequestSoapClient())
            {
                responseXmlData = client.Submit(stringWriter.ToString());
            }

            // Process response
            XmlDocument responseXml = new XmlDocument();
            responseXml.LoadXml(responseXmlData);

            verifyResponseXmlData(responseXml);

            XmlNode jobIdNode = responseXml.SelectSingleNode("Screen/Document/@JobID");

            if (jobIdNode != null)
            {
                jobId = jobIdNode.Value;
            }

            return jobId;
        }

        // Send Request to any Network Printer.

        public string SendToPrinter(string device, string dealId, string filePath, string userId)
        {
            string jobId = null;
            ScreenSysServRequestDestinationMethodsPRINT destMethod = new ScreenSysServRequestDestinationMethodsPRINT();
            destMethod.Device = device;

            Screen screen = new Screen();
            screen.SysServRequest.Document.SourceURL = filePath;
            screen.SysServRequest.Application = "LLC";
            screen.SysServRequest.RequestorID = userId;
            screen.SysServRequest.DealID = dealId;
            screen.SysServRequest.Document.ConvertToPDF = "True";
            screen.SysServRequest.Document.ConvertToText = "False";

            screen.SysServRequest.Document.DocType = "LLC";
            screen.SysServRequest.Document.InfoSource = "NONE";
            screen.SysServRequest.Document.ModelDocument = "NONE";

            screen.SysServRequest.Destination.Methods.PRINT = destMethod;

            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Screen));
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            xmlSerializer.Serialize(stringWriter, screen);
            string requestXmlData = stringWriter.ToString();

            // Submit request
            string responseXmlData = String.Empty;
            using (SystemService.RequestSoapClient client = new SystemService.RequestSoapClient())
            {
                responseXmlData = client.Submit(requestXmlData);
            }

            // Process response
            XmlDocument responseXml = new XmlDocument();
            responseXml.LoadXml(responseXmlData);

            verifyResponseXmlData(responseXml);

            XmlNode jobIdNode = responseXml.SelectSingleNode("Screen/Document/@JobID");

            if (jobIdNode != null)
            {
                jobId = jobIdNode.Value;
            }

            return jobId;
        }

        /// <summary>
        /// Uploads the documents contained in the documentList structure into DMS system
        /// </summary>
        /// <param name="documentUrl"></param>
        /// <param name="dmsPath"></param>
        /// <param name="categoryName"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public string GenerateDocument(string documentUrl, string dmsPath, string dealId, string userId, string documentDealId, string documentDocId)
        {
            if (dmsPath == null || dmsPath.Length == 0)
            {
                throw new ArgumentNullException("dmsPath");
            }

            string jobId = null;
            //Prepare XML for the Systems Services request
            ScreenSysServRequestDestinationMethodsSTORE destMethod = new ScreenSysServRequestDestinationMethodsSTORE();
            destMethod.DMSPath = dmsPath;

            Screen screen = new Screen();
            screen.SysServRequest.Document.SourceURL = documentUrl;

            screen.SysServRequest.Application = "LLC";
            screen.SysServRequest.RequestorID = userId;
            screen.SysServRequest.DealID = dealId;
            screen.SysServRequest.Document.ConvertToPDF = "False";
            screen.SysServRequest.Document.ConvertToText = "False";
            screen.SysServRequest.Document.DocType = "LLC";
            screen.SysServRequest.Document.InfoSource = "NewLLC";
            screen.SysServRequest.Document.ModelDocument = "LLCMaster.CMS";
            ScreenSysServRequestDocumentDocumentDataDocumentID dataDocumentId = new ScreenSysServRequestDocumentDocumentDataDocumentID();

            dataDocumentId.DataType = "Numeric";
            dataDocumentId.Value = documentDocId;
            screen.SysServRequest.Document.DocumentData.DocumentId = dataDocumentId;
            ScreenSysServRequestDocumentDocumentDataDealId dataDealId = new ScreenSysServRequestDocumentDocumentDataDealId();
            dataDealId.DataType = "Numeric";
            dataDealId.Value = documentDealId;
            screen.SysServRequest.Document.DocumentData.DealId = dataDealId;

            screen.SysServRequest.Destination.Methods.STORE = destMethod;

            // Serialize the request object
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Screen));
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            xmlSerializer.Serialize(stringWriter, screen);

            string requestXmlData = stringWriter.ToString();

            // Submit request
            string responseXmlData = String.Empty;
            using (SystemService.RequestSoapClient client = new SystemService.RequestSoapClient())
            {
                responseXmlData = client.Submit(stringWriter.ToString());
            }

            // Process response
            XmlDocument responseXml = new XmlDocument();
            responseXml.LoadXml(responseXmlData);

            verifyResponseXmlData(responseXml);

            XmlNode jobIdNode = responseXml.SelectSingleNode("Screen/Document/@JobID");

            if (jobIdNode != null)
            {
                jobId = jobIdNode.Value;
            }

            return jobId;
        }

        public string UploadDocument(string documentUrl, string dmsPath, string dealId, string userId)
        {
            // Validate parameters
            if (documentUrl == null || documentUrl.Length == 0)
            {
                throw new ArgumentNullException("documentUrl");
            }

            if (dmsPath == null || dmsPath.Length == 0)
            {
                throw new ArgumentNullException("dmsPath");
            }

            string jobId = null;

            //Prepare XML for the Systems Services request
            ScreenSysServRequestDestinationMethodsSTORE destMethod = new ScreenSysServRequestDestinationMethodsSTORE();
            destMethod.DMSPath = dmsPath;

            Screen screen = new Screen();
            screen.SysServRequest.Document.SourceURL = documentUrl;
            screen.SysServRequest.Application = "LLC";
            screen.SysServRequest.RequestorID = userId;
            screen.SysServRequest.DealID = dealId;
            screen.SysServRequest.Document.ConvertToPDF = "False";
            screen.SysServRequest.Document.ConvertToText = "False";
            screen.SysServRequest.Document.DocType = "LLC";
            screen.SysServRequest.Document.InfoSource = "NONE";
            screen.SysServRequest.Document.ModelDocument = "NONE";

            screen.SysServRequest.Destination.Methods.STORE = destMethod;

            // Serialize the request object
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Screen));
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            xmlSerializer.Serialize(stringWriter, screen);

            string requestXmlData = stringWriter.ToString();

            // Submit request
            string responseXmlData = String.Empty;
            using (SystemService.RequestSoapClient client = new SystemService.RequestSoapClient())
            {
                responseXmlData = client.Submit(stringWriter.ToString());
            }

            // Process response
            XmlDocument responseXml = new XmlDocument();
            responseXml.LoadXml(responseXmlData);

            verifyResponseXmlData(responseXml);

            XmlNode jobIdNode = responseXml.SelectSingleNode("Screen/Document/@JobID");

            if (jobIdNode != null)
            {
                jobId = jobIdNode.Value;
            }

            return jobId;
        }

        /// <summary>
        /// Retrieves the content of the specified document from DMS
        /// </summary>
        /// <param name="dmsPath"></param>
        /// <returns></returns>
        public byte[] GetDocument(string dmsPath)
        {
            return GetDocument(dmsPath, String.Empty);
        }

        public string SendFax(string faxNumber, string dealId, string coverPage, string message, string filePath, string userId)
        {
            string jobId = null;
            ScreenSysServRequestDestinationMethodsFAX destMethod = new ScreenSysServRequestDestinationMethodsFAX();
            destMethod.FaxNumber = faxNumber;
            destMethod.CoverPage = coverPage;

            destMethod.Message = message;
            Screen screen = new Screen();
            screen.SysServRequest.Document.SourceURL = filePath;
            screen.SysServRequest.Application = "EPS";
            screen.SysServRequest.RequestorID = userId;
            screen.SysServRequest.DealID = dealId;
            screen.SysServRequest.Document.ConvertToPDF = "True";
            screen.SysServRequest.Document.ConvertToText = "False";

            screen.SysServRequest.Document.DocType = "EPS";
            screen.SysServRequest.Document.InfoSource = "NONE";
            screen.SysServRequest.Document.ModelDocument = "NONE";

            screen.SysServRequest.Destination.Methods.FAX = destMethod;

            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Screen));
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            xmlSerializer.Serialize(stringWriter, screen);
            string requestXmlData = stringWriter.ToString();

            // Submit request
            string responseXmlData = String.Empty;
            using (SystemService.RequestSoapClient client = new SystemService.RequestSoapClient())
            {
                responseXmlData = client.Submit(requestXmlData);
            }

            // Process response
            XmlDocument responseXml = new XmlDocument();
            responseXml.LoadXml(responseXmlData);

            verifyResponseXmlData(responseXml);

            XmlNode jobIdNode = responseXml.SelectSingleNode("Screen/Document/@JobID");

            if (jobIdNode != null)
            {
                jobId = jobIdNode.Value;
            }

            return jobId;
        }

        public string SendEmail(string emailAddress, string dealId, string subject, string message, string filePath, string userId)
        {
            string jobId = null;
            ScreenSysServRequestDestinationMethodsEMAIL destMethod = new ScreenSysServRequestDestinationMethodsEMAIL();
            destMethod.EmailAddress = emailAddress;
            destMethod.Subject = subject;

            destMethod.NoAttachment = string.IsNullOrEmpty(filePath) ? "True":"False";

            destMethod.Message = message;
            Screen screen = new Screen();
            screen.Action = "EMAIL";
            screen.SysServRequest.Document.SourceURL = string.IsNullOrEmpty(filePath) ? "NONE" : filePath;
            screen.SysServRequest.Application = "LLC";
            screen.SysServRequest.RequestorID = userId;
            screen.SysServRequest.DealID = dealId;
            screen.SysServRequest.Document.ConvertToPDF = "False";
            screen.SysServRequest.Document.ConvertToText = "False";

            screen.SysServRequest.Document.DocType = "NONE";
            screen.SysServRequest.Document.InfoSource = "NONE";
            screen.SysServRequest.Document.ModelDocument = "NONE";

            screen.SysServRequest.Destination.Methods.EMAIL = destMethod;

            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Screen));
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            xmlSerializer.Serialize(stringWriter, screen);
            string requestXmlData = stringWriter.ToString();

            // Submit request
            string responseXmlData = String.Empty;
            using (SystemService.RequestSoapClient client = new SystemService.RequestSoapClient())
            {
                responseXmlData = client.Submit(requestXmlData);
            }

            // Process response
            XmlDocument responseXml = new XmlDocument();
            responseXml.LoadXml(responseXmlData);

            verifyResponseXmlData(responseXml);

            XmlNode jobIdNode = responseXml.SelectSingleNode("Screen/Document/@JobID");

            if (jobIdNode != null)
            {
                jobId = jobIdNode.Value;
            }

            return jobId;
        }


        public string GetDocumentPath(string dmsPath, string Version)
        {
            // Prepare request
            ScreenSysServRequestDestinationMethodsRETRIEVE destMethod = new ScreenSysServRequestDestinationMethodsRETRIEVE();
            destMethod.DMSPath = dmsPath;
            if (Version != "")
                destMethod.Version = Version;

            //destMethod.Version = "1";
            Screen screen = new Screen();
            screen.SysServRequest.Application = "LLC";
            screen.SysServRequest.Destination.Methods.RETRIEVE = destMethod;

            // Serialize the request object
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Screen));
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            xmlSerializer.Serialize(stringWriter, screen);

            // Submit request
            string responseXmlData = String.Empty;
            using (SystemService.RequestSoapClient client = new SystemService.RequestSoapClient())
            {
                responseXmlData = client.Submit(stringWriter.ToString());
            }

            XmlDocument responseXml = new XmlDocument();
            responseXml.LoadXml(responseXmlData);

            XmlNode fileNameNode = responseXml.SelectSingleNode("Screen/Document");
            if (fileNameNode == null)
            {
                throw new Exception(String.Format("File {0} not found. Screen/Document path not found in response from system services", dmsPath));
            }

            string fileName = fileNameNode.InnerText;

            if (fileName == null || fileName.Length == 0)
            {
                throw new Exception(String.Format("File {0} not found", dmsPath));
            }
            return fileName;
        }

        /// <summary>
        /// Retrieves the content of the specified document from DMS
        /// </summary>
        /// <param name="dmsPath"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public byte[] GetDocument(string dmsPath, string version)
        {
            // Prepare request
            ScreenSysServRequestDestinationMethodsRETRIEVE destMethod = new ScreenSysServRequestDestinationMethodsRETRIEVE();
            destMethod.DMSPath = dmsPath;
            //destMethod.Version = "1";
            Screen screen = new Screen();
            screen.SysServRequest.Application = "LLC";
            screen.SysServRequest.Destination.Methods.RETRIEVE = destMethod;

            // Serialize the request object
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Screen));
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            xmlSerializer.Serialize(stringWriter, screen);

            // Submit request
            string responseXmlData = String.Empty;
            using (SystemService.RequestSoapClient client = new SystemService.RequestSoapClient())
            {
                responseXmlData = client.Submit(stringWriter.ToString());
            }

            XmlDocument responseXml = new XmlDocument();
            responseXml.LoadXml(responseXmlData);

            XmlNode fileNameNode = responseXml.SelectSingleNode("Screen/Document");
            if (fileNameNode == null)
            {
                throw new Exception(String.Format("File {0} not found. Screen/Document path not found in response from system services", dmsPath));
            }

            string fileName = fileNameNode.InnerText;

            if (fileName == null || fileName.Length == 0)
            {
                throw new Exception(String.Format("File {0} not found", dmsPath));
            }

            byte[] result = null;

            try
            {
                //Retrieve file from URL
                WebRequest request = WebRequest.Create(fileName);
                WebResponse response = request.GetResponse();

                //Read bynary stream
                System.IO.BinaryReader br = new System.IO.BinaryReader(response.GetResponseStream());
                result = br.ReadBytes((int)response.ContentLength);
                br.Close();
            }
            //TODO: Handle exception: Log exception before rethrowing, origninal problem lost
            catch (Exception exc)
            {
                throw new Exception("Error accessing " + fileName, exc);
            }

            return result;
        }

        /// <summary>
        /// Returns the status of the specified jobId
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public string GetJobStatus(string jobId)
        {
            string result = "";

            string requestXmlData = String.Format(
                "<Screen Action=\"{0}\" Application=\"{1}\" JobID=\"{2}\"/>",
                "QueryJob",
                "LLC",
                jobId);

            string responseXmlData = String.Empty;
            using (SystemService.RequestSoapClient client = new SystemService.RequestSoapClient())
            {
                responseXmlData = client.QueryStatus(requestXmlData);
            }

            XmlDocument responseXml = new XmlDocument();
            responseXml.LoadXml(responseXmlData);

            XmlNode errorCodeNode = responseXml.SelectSingleNode("Screen/@ErrNumber");

            string errorCode = errorCodeNode.Value;
            if (errorCode == "-1")
            {
                XmlNode errorDescNode = responseXml.SelectSingleNode("Screen/@ErrDesc");

                if (errorDescNode == null)
                {
                    throw new Exception(
                        "System Services has returned error code -1 for the requested operation but no description attribute node is available" + " , Request XML: " + requestXmlData);
                }

                string errorDesc = errorDescNode.InnerText;

                if (errorDesc == null || errorDesc.Length == 0)
                {
                    throw new Exception(
                        "System Services has returned error code -1 for the requested operation but no description message is available" + " , Request XML: " + requestXmlData);
                }

                if (errorDesc.ToLower().EndsWith("not found."))
                {
                    result = "Failed";
                }

                throw new Exception(
                    "System Services has returned error code -1 with description " + errorDesc + " , Request XML: " + requestXmlData);
            }
            else
            {
                XmlNode statusNode = responseXml.SelectSingleNode(@"Screen/Job/Status");

                if (statusNode != null && statusNode.InnerText != null)
                {
                    switch (statusNode.InnerText.ToLower())
                    {
                        case "stored":
                            result = "Successful";
                            break;

                        case "failed":
                            result = "Failed";
                            break;
                        case "pending":
                            result = "Pending";
                            break;
                        default:
                            result = statusNode.InnerText.ToLower();
                            break;
                    }
                }
            }

            return result;
        }

        #region Private Methods

        /// <summary>
        /// Validates the response object received from DMS
        /// </summary>
        /// <param name="responseXmlData"></param>
        private void verifyResponseXmlData(string responseXmlData)
        {
            XmlDocument responseXml = new XmlDocument();
            responseXml.LoadXml(responseXmlData);

            verifyResponseXmlData(responseXml);
        }

        /// <summary>
        /// Validates the response object received from DMS
        /// </summary>
        /// <param name="responseXml"></param>
        private void verifyResponseXmlData(XmlDocument responseXml)
        {
            if (responseXml == null)
            {
                throw new ArgumentNullException("responseXml");
            }

            XmlNode errorCodeNode = responseXml.SelectSingleNode("Screen/@ErrNumber");
            if (errorCodeNode == null)
            {
                throw new Exception("System Services has returned an unexpected response for the ERASE document request. X-path 'Screen/@ErrNumber' not found.");
            }

            string errorCode = errorCodeNode.Value;
            if (errorCode.Length > 0 && errorCode != "0")
            {
                XmlNode errorDescNode = responseXml.SelectSingleNode("Screen/@ErrDesc");

                throw new Exception(String.Format(
                    "System Services has returned error code '{0}' for the requested operation, description : {1}",
                    errorCode, errorDescNode.InnerText));
            }
        }

        #endregion Private Methods

    }


}