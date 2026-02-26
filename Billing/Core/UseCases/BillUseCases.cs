/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Use cases Layer                         *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Use case interactor class               *
*  Type     : BillUseCases                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases to manage billing.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;
using Empiria.Financial;
using Empiria.Services;

using Empiria.Billing.SATMexicoImporter;

using Empiria.Billing.Adapters;
using Empiria.Billing.Data;
using System.Collections.Generic;

namespace Empiria.Billing.UseCases {

  /// <summary>Use cases to manage billing.</summary>
  public class BillUseCases : UseCase {

    #region Constructors and parsers

    protected BillUseCases() {
      // no-op
    }

    static public BillUseCases UseCaseInteractor() {
      return UseCase.CreateInstance<BillUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public Bill CreateCFDI(string xmlString, IPayableEntity payable, DocumentProduct billProduct) {
      Assertion.Require(xmlString, nameof(xmlString));
      Assertion.Require(payable, nameof(payable));
      Assertion.Require(billProduct, nameof(billProduct));

      string billType = billProduct.ApplicationContentType;

      if (billType == "factura-electronica-sat") {
        return CreateBill(xmlString, payable);

      } else if (billType == "nota-credito-sat") {
        return CreateCreditNote(xmlString, payable, false);

      } else if (billType == "nota-credito-penalizacion-sat") {
        return CreateCreditNote(xmlString, payable, true);

      } else if (billType == "complemento-pago-sat") {
        return CreateBillPaymentComplement(xmlString, payable);

      } else if (billType == "factura-consumo-combustible-sat") {
        return CreateFuelConsumptionBill(xmlString, payable, billType);

      } else {
        throw Assertion.EnsureNoReachThisCode($"Unrecognized applicationContentType '{billType}'.");
      }
    }


    public Bill CreateFuelConsumptionBill(string xmlString, IPayableEntity payable, string billType) {
      Assertion.Require(xmlString, nameof(xmlString));
      Assertion.Require(payable, nameof(payable));

      var reader = new SATFuelConsumptionBillXmlReader(xmlString);

      ISATBillDto satDto = reader.ReadAsFuelConsumptionBillDto();

      IBillFields fields = FuelConsumptionBillMapper.Map((SATFuelConsumptionBillDto) satDto, billType);

      return CreateFuelConsumptionImplementation(payable, (FuelConsumptionBillFields) fields);
    }


    public Bill CreateVoucherBill(IPayableEntity payable, DocumentFields fields) {
      Assertion.Require(payable, nameof(payable));
      Assertion.Require(fields, nameof(fields));

      Assertion.Require(fields.DocumentNumber, "Requiero el número de oficio o documento que ampara al comprobante.");
      Assertion.Require(fields.Name, "Requiero la descripción del oficio o documento que ampara al comprobante.");
      Assertion.Require(fields.Total, "Requiero el total del comprobante");

      var billCategory = BillCategory.Parse(fields.DocumentProductUID);

      var bill = new Bill(payable, billCategory, fields);

      bill.Save();

      return bill;
    }


    public void DeleteBill(string billUID) {

      Bill bill = Bill.Parse(billUID);

      bill.Delete();

      bill.Save();
    }


    public string ExtractCFDINo(string xmlString) {
      Assertion.Require(xmlString, nameof(xmlString));

      var reader = new SATBillXmlReader(xmlString);

      SATBillDto satDto = reader.ReadAsBillDto();

      return satDto.SATComplemento.UUID;
    }


    public FixedList<Bill> SearchBills(string filter, string sort) {

      return BillData.SearchBills(filter, sort);
    }

    #endregion Use cases

    #region Private methods

    private Bill CreateBill(string xmlString, IPayableEntity payable) {
      Assertion.Require(xmlString, nameof(xmlString));
      Assertion.Require(payable, nameof(payable));

      var reader = new SATBillXmlReader(xmlString);

      ISATBillDto satDto = reader.ReadAsBillDto();

      BillFields fields = (BillFields) BillFieldsMapper.Map((SATBillDto) satDto);

      return CreateBillByCategory(payable, fields, BillCategory.FacturaProveedores);
    }


    private Bill CreateBillByCategory(IPayableEntity payable, BillFields fields, BillCategory billCategory) {

      var bill = new Bill(payable, billCategory, fields.BillNo);

      bill.Update(fields);

      bill.Save();

      foreach (var fieldsConcept in fields.Concepts) {
        bill.AddConcept(BillConceptType.Default, fieldsConcept);
      }

      foreach (var addendaConcept in fields.Addenda.Concepts) {
        bill.AddConcept(BillConceptType.Addenda, addendaConcept);
      }

      foreach (var billTax in fields.BillTaxes) {
        bill.AddBillTaxes(billTax, -1);
      }

      return bill;
    }


    private Bill CreateBillPaymentComplement(string xmlString, IPayableEntity payable) {
      Assertion.Require(xmlString, nameof(xmlString));
      Assertion.Require(payable, nameof(payable));

      var reader = new SATPaymentComplementXmlReader(xmlString);

      ISATBillDto satDto = reader.ReadAsPaymentComplementDto();

      IBillFields fields = BillPaymentComplementFieldsMapper.Map((SatBillPaymentComplementDto) satDto);

      return CreatePaymentComplementImplementation(payable, (BillPaymentComplementFields) fields);
    }


    private Bill CreateCreditNote(string xmlString, IPayableEntity payable, bool isForPenalty) {
      Assertion.Require(xmlString, nameof(xmlString));
      Assertion.Require(payable, nameof(payable));

      var reader = new SATCreditNoteXmlReader(xmlString);

      ISATBillDto satDto = reader.ReadAsCreditNoteDto();

      BillFields fields = (BillFields) BillFieldsMapper.Map((SATBillDto) satDto);

      if (isForPenalty) {
        return CreateBillByCategory(payable, fields, BillCategory.NotaDeCreditoPenalizacion);
      } else {
        return CreateBillByCategory(payable, fields, BillCategory.NotaDeCreditoProveedores);
      }
    }


    private Bill CreateFuelConsumptionImplementation(IPayableEntity payable, FuelConsumptionBillFields fields) {

      var billCategory = BillCategory.FacturaConsumoCombustible;

      var bill = new Bill(payable, billCategory, fields.BillNo);

      bill.UpdateFuelConsumptionBill(fields);

      bill.Save();

      foreach (var fieldsConcept in fields.Concepts) {
        bill.AddConcept(BillConceptType.Default, fieldsConcept);
      }

      foreach (var fieldsConcept in fields.ComplementData.ComplementConcepts) {
        bill.AddComplementConcepts(fieldsConcept);
      }

      foreach (var fieldsConcept in fields.Addenda.Concepts) {
        
        if (fieldsConcept.IsBonusConcept) {
          bill.AddConcept(BillConceptType.Addenda, fieldsConcept);
        }
      }

      return bill;
    }


    private Bill CreatePaymentComplementImplementation(IPayableEntity payable, BillPaymentComplementFields fields) {

      var billCategory = BillCategory.ComplementoPagoProveedores;

      var bill = new Bill(payable, billCategory, fields.BillNo);

      bill.UpdatePaymentComplement(fields);

      bill.Save();

      foreach (var fieldsConcept in fields.Concepts) {
        bill.AddConcept(BillConceptType.Default, fieldsConcept);
      }

      foreach (var relatedPayoutFields in fields.ComplementRelatedPayoutData) {
        bill.AddBillRelatedBill(relatedPayoutFields);
      }

      return bill;
    }

    #endregion Private methods

  } // class BillUseCases

} // namespace Empiria.Billing.UseCases
