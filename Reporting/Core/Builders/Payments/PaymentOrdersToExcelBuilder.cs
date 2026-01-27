/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                           Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Report builder                       *
*  Type     : PaymentOrdersToExcelBuilder                   License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds an Excel file with payment orders.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Office;
using Empiria.Storage;

namespace Empiria.Payments.Reporting {

  /// <summary>Builds an Excel file with payment order entries.</summary>
  internal class PaymentOrdersToExcelBuilder {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;

    public PaymentOrdersToExcelBuilder(FileTemplateConfig templateConfig) {
      Assertion.Require(templateConfig, nameof(templateConfig));

      _templateConfig = templateConfig;
    }


    internal ExcelFile CreateExcelFile(FixedList<PaymentOrder> paymentOrders) {
      Assertion.Require(paymentOrders, nameof(paymentOrders));

      _excelFile = new ExcelFile(_templateConfig);

      _excelFile.Open();

      SetHeader();

      FillOut(paymentOrders);

      _excelFile.Save();

      _excelFile.Close();

      return _excelFile;
    }


    private void SetHeader() {
      _excelFile.SetCell(_templateConfig.TitleCell, _templateConfig.Title);
      _excelFile.SetCell(_templateConfig.CurrentTimeCell,
                         $"Generado el: {DateTime.Now.ToString("dd/MMM/yyyy HH:mm")}");
    }


    private void FillOut(FixedList<PaymentOrder> paymentOrders) {
      int i = _templateConfig.FirstRowIndex;

      foreach (var order in paymentOrders) {
        _excelFile.SetCell($"A{i}", order.PaymentOrderNo);
        _excelFile.SetCell($"B{i}", order.RequestedBy.Code);
        _excelFile.SetCell($"C{i}", order.RequestedBy.Name);
        _excelFile.SetCell($"E{i}", order.PaymentType.Name);
        _excelFile.SetCell($"D{i}", order.PaymentMethod.Name);
        _excelFile.SetCell($"F{i}", order.PayTo.Name);
        _excelFile.SetCell($"G{i}", order.PayableEntity.EntityNo);
        _excelFile.SetCell($"H{i}", order.PayableEntity.Name);
        _excelFile.SetCell($"I{i}", order.PayableEntity.Budget.Name);
        _excelFile.SetCell($"J{i}", order.Currency.ISOCode);
        _excelFile.SetCell($"K{i}", order.Total);
        _excelFile.SetCell($"L{i}", order.PaymentAccount.Institution.Name);
        _excelFile.SetCell($"M{i}", order.PaymentAccount.AccountNo);
        _excelFile.SetCell($"N{i}", order.ReferenceNumber);
        _excelFile.SetCell($"O{i}", order.Debtor.Name);
        _excelFile.SetCell($"P{i}", order.PostedBy.Name);
        _excelFile.SetCell($"Q{i}", order.PostingTime.ToString("dd/MMM/yyyy HH:mm"));
        _excelFile.SetCell($"R{i}", order.Status.GetName());

        if (order.Payed) {
          _excelFile.SetCell($"S{i}", order.LastPaymentInstruction.LastUpdateTime.ToString("dd/MMM/yyyy HH:mm"));
          _excelFile.SetCell($"T{i}", order.LastPaymentInstruction.BrokerInstructionNo);
        }

        i++;
      }
    }

  } // class PaymentOrdersToExcelBuilder

} // namespace Empiria.Payments.Reporting
