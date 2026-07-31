/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payment documentation services             Component : Service Layer                           *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Service                                 *
*  Type     : PaymentDocumentation                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Collector of Payment documentation  by specific date.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.IO;
using System.IO.Compression;

using Empiria.Billing;
using Empiria.Documents;
using Empiria.Payments.Data;

namespace Empiria.Payments {

  /// <summary>Engine that processes payment instructions.</summary>
  public class PaymentDocumentation {
    #region Properties and Fields

    static private string BASE_DIRECTORY = ConfigurationData.GetString("Storage", "Reports.GenerationStoragePath");

    static int voucherCount = 0;

    #endregion Properties and Fields

    #region Methods

    static public string Proccess(DateTime fromDate, DateTime toDate) {
      var paymentOrders = GetPaymentOrders(fromDate, toDate);

      string zipFileName = BuildPaymentsPackage(paymentOrders);
      return zipFileName;
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
          fileName = dctosDirectory + "\\" + SetFileName(paymentOrder, bill, document);
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
