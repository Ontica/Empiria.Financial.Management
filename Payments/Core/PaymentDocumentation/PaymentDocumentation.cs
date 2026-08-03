/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payment documentation services             Component : Service Layer                           *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Static Service                          *
*  Type     : PaymentDocumentation                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Generates a ZipFile with payments documentation by a given date period.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.IO;
using System.IO.Compression;

using Empiria.Documents;
using Empiria.Storage;

using Empiria.Billing;

using Empiria.Payments.Data;

namespace Empiria.Payments {

  /// <summary>Engine that generates a ZipFile with payments documentation by a given date period.</summary>
  static public class PaymentDocumentation {

    #region Properties and Fields

    static private string BASE_DIRECTORY = ConfigurationData.GetString("Empiria.Storage",
                                                                       "Reports.GenerationStoragePath");
    static private string HTTP_BASE_URL = ConfigurationData.GetString("Empiria.Storage",
                                                                      "Reports.BaseUrl");

    static int voucherCount = 0;

    #endregion Properties and Fields

    #region Methods

    static public FileDto GenerateZipFile(DateTime fromDate, DateTime toDate) {

      var paymentOrders = GetPaymentOrders(fromDate, toDate);

      string zipFileName = BuildPaymentsPackage(paymentOrders);


      return new FileDto(FileType.Zip, $"{HTTP_BASE_URL}/{zipFileName}");
    }

    #endregion Methods

    #region Helpers

    static private string BuildPaymentsPackage(FixedList<PaymentOrder> paymentOrders) {
      string folderName = "DoctosPagos_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");

      string path = Path.Combine(BASE_DIRECTORY, folderName);

      CreateDirectory(path);

      foreach (var paymentOrder in paymentOrders) {
        LoadBills(paymentOrder, path);
      }

      string zipPath = path + ".zip";

      ZipFolder(path, zipPath);

      return Path.GetFileName(zipPath);
    }


    static private void CreateDirectory(string directoryPath) {

      if (!Directory.Exists(directoryPath)) {
        Directory.CreateDirectory(directoryPath);
      }
    }


    static private void CopyFile(string source, string dest) {
      try {
        File.Copy(source, dest, true);
      } catch (Exception ex) {
        throw (ex);
      }
    }


    static private FixedList<PaymentOrder> GetPaymentOrders(DateTime fromDate, DateTime toDate) {

      return PaymentDocumentationData.GetPaymentOrders(fromDate, toDate);
    }


    static private void LoadBills(PaymentOrder paymentOrder, string directory) {
      string dctosDirectory = Path.Combine(directory, paymentOrder.PaymentOrderNo);
      string fileName = string.Empty;
      string destFile = string.Empty;

      CreateDirectory(dctosDirectory);
      voucherCount = 0;

      var bills = Bill.GetListFor(paymentOrder.PayableEntity);

      foreach (var bill in bills) {
        var documents = Documents.DocumentServices.GetEntityDocuments(bill);

        foreach (var document in documents) {
          fileName = Path.Combine(dctosDirectory, SetFileName(paymentOrder, bill, document));
          destFile = Path.Combine(directory, fileName);

          CopyFile(document.FullLocalName, destFile);
        }
      }

    }


    static private string SetFileName(PaymentOrder paymentOrder, Bill bill, DocumentDto document) {
      string fileName = paymentOrder.PaymentOrderNo + ".";
      string fileExtension = Path.GetExtension(document.FullLocalName);

      switch (bill.BillType.Id) {
        case 3422:
        case 3423:
        case 3424: {
          voucherCount++;
          return fileName + document.ApplicationContentType + "-" + $"{voucherCount:D3}" + fileExtension;
        }
        default:
          return fileName + document.DocumentNo + fileExtension;
      }
    }


    static private void ZipFolder(string source, string dest) {
      try {
        ZipFile.CreateFromDirectory(source, dest, CompressionLevel.Optimal, includeBaseDirectory: false);
      } catch (Exception ex) {
        throw (ex);
      }
    }


    #endregion Helpers

  }  // class PaymentDocumentation

} // namespace Empiria.Payments
