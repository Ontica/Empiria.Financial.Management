/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Management                             Component : Reporting Services                   *
*  Assembly : Empiria.Financial.Reporting.Core.dll          Pattern   : Service provider                     *
*  Type     : PaymentsBillsReportBuilder                    License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds a dynamic report with all payments in a time period with their payed bills.             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;

using Empiria.DynamicData;

using Empiria.Billing;

namespace Empiria.Payments.Reporting {

  public class PaymentBillDto {

    public string UID {
      get; internal set;
    }

    public string PaymentOrderNo {
      get; internal set;
    }

    public string PayToName {
      get; internal set;
    }

    public string PayToCode {
      get; internal set;
    }

    public DateTime PaymentDate {
      get; internal set;
    }

    public string PaymentMethod {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

    public string OrgUnitCode {
      get; internal set;
    }

    public string OrgUnitName {
      get; internal set;
    }
    public string BillType {
      get;
      internal set;
    }
    public object BillNo {
      get;
      internal set;
    }
    public object BillSubtotal {
      get;
      internal set;
    }
    public object BillIVA {
      get;
      internal set;
    }
    public object BillISR {
      get;
      internal set;
    }
    public decimal BillTotal {
      get;
      internal set;
    }
  }  // class PaymentBillDto



  /// <summary>Builds a dynamic report with all payments in a time period with their payed bills.</summary>
  public class PaymentsBillsReportBuilder {

    private readonly DateTime _fromDate;
    private readonly DateTime _toDate;

    public PaymentsBillsReportBuilder(DateTime fromDate, DateTime toDate) {
      _fromDate = fromDate;
      _toDate = toDate;
    }

    internal FixedList<DataTableColumn> BuildColumns() {
      return new List<DataTableColumn>() {
        new DataTableColumn("paymentOrderNo", "Solicitud de pago", "text"),
        new DataTableColumn("payToName", "Beneficiario", "text"),
        new DataTableColumn("payToCode", "RFC", "text"),
        new DataTableColumn("paymentDate", "Fecha de pago", "date"),
        new DataTableColumn("paymentMethod", "Forma de pago", "text"),
        new DataTableColumn("total", "Total pagado", "decimal"),
        new DataTableColumn("orgUnitCode", "Area", "text"),
        new DataTableColumn("orgUnitName", "Nombre área", "text"),
        new DataTableColumn("billType", "Comprobante", "text"),
        new DataTableColumn("billNo", "Número", "text"),
        new DataTableColumn("billSubtotal", "Subtotal", "decimal"),
        new DataTableColumn("billIVA", "IVA", "decimal"),
        new DataTableColumn("billISR", "ISR", "decimal"),
        new DataTableColumn("billTotal", "Total", "decimal"),
      }.ToFixedList();
    }


    internal FixedList<PaymentBillDto> BuildEntries() {
      FixedList<PaymentOrder> paymentOrders = GetPaymentOrders();

      return paymentOrders.SelectFlat(x => CreatePaymentConceptsDto(x));
    }


    #region Helpers

    private FixedList<PaymentBillDto> CreatePaymentConceptsDto(PaymentOrder paymentOrder) {

      var bills = Bill.GetListFor(paymentOrder.PayableEntity);

      return bills.Select(bill => CreatePaymentConceptDto(paymentOrder, bill))
                  .ToFixedList();
    }


    private PaymentBillDto CreatePaymentConceptDto(PaymentOrder paymentOrder, Bill bill) {

      decimal iva = bill.Taxes;

      return new PaymentBillDto {
        UID = paymentOrder.UID,
        PaymentOrderNo = paymentOrder.PaymentOrderNo,
        PayToName = paymentOrder.PayTo.Name,
        PayToCode = paymentOrder.PayTo.Code,
        PaymentDate = paymentOrder.PostingTime,
        PaymentMethod = paymentOrder.PaymentMethod.Name,
        Total = paymentOrder.Total,
        OrgUnitCode = paymentOrder.RequestedBy.Code,
        OrgUnitName = paymentOrder.RequestedBy.Name,
        BillType = bill.BillCategory.Name,
        BillNo = bill.BillNo,
        BillSubtotal = bill.Subtotal,
        BillIVA = iva,
        BillISR = 0m,
        BillTotal = bill.Total,
      };
    }


    private FixedList<PaymentOrder> GetPaymentOrders() {
      return PaymentOrder.GetList<PaymentOrder>()
                         .FindAll(x => x.Status == PaymentOrderStatus.Payed &&
                                                   _fromDate <= x.PostingTime && x.PostingTime < _toDate.AddDays(1))
                         .ToFixedList()
                         .Sort((x, y) => x.PostingTime.CompareTo(y.PostingTime));
    }


    #endregion Helpers

  }  // class PaymentsBillsReportBuilder

}  // namespace Empiria.Payments.Reporting
