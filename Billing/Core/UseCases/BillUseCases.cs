/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Use cases Layer                         *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Use case interactor class               *
*  Type     : BillUseCases                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases to manage billing.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;

using Empiria.Financial;
using Empiria.Products;
using Empiria.Services;

using Empiria.Billing.SATMexicoImporter;

using Empiria.Billing.Adapters;
using Empiria.Billing.Data;
using System.Linq;
using System;

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


    public BillDto CreateBillTest(string xmlString) {
      Assertion.Require(xmlString, nameof(xmlString));

      //var reader = new SATBillXmlReader(xmlString);
      //SATBillDto satDto = reader.ReadAsBillDto();

      var reader = new SATCreditNoteXmlReader(xmlString);
      SATBillDto satDto = reader.ReadAsCreditNoteDto();

      BillFields fields = BillFieldsMapper.Map(satDto);

      Bill bill = CreateBillTest(fields);

      return BillMapper.MapToBillDto(bill);
    }


    public BillDto CreateBill(string xmlString, IPayable payable) {
      Assertion.Require(xmlString, nameof(xmlString));
      Assertion.Require(payable, nameof(payable));
      
      var reader = new SATBillXmlReader(xmlString);
      
      SATBillDto satDto = reader.ReadAsBillDto();

      BillFields fields = BillFieldsMapper.Map(satDto);

      Bill bill = CreateBillImplementation(payable, fields);

      return BillMapper.MapToBillDto(bill);
    }


    public BillDto CreateCreditNote(string xmlString, IPayable payable) {
      Assertion.Require(xmlString, nameof(xmlString));
      Assertion.Require(payable, nameof(payable));

      var reader = new SATCreditNoteXmlReader(xmlString);

      SATBillDto satDto = reader.ReadAsCreditNoteDto();

      BillFields fields = BillFieldsMapper.Map(satDto);

      Bill bill = CreateCreditNoteImplementation(payable, fields);

      return BillMapper.MapToBillDto(bill);
    }


    public BillHolderDto GetBill(string billUID) {
      Assertion.Require(billUID, nameof(billUID));

      Bill bill = Bill.Parse(billUID);
      bill.AssignConcepts();

      return BillMapper.Map(bill);
    }


    public BillWithConceptsDto GetBillWithConceptsDto(string billUID) {
      Assertion.Require(billUID, nameof(billUID));

      Bill bill = Bill.Parse(billUID);
      bill.AssignConcepts();

      return BillMapper.MapToBillWithConcepts(bill);
    }


    public FixedList<BillDescriptorDto> GetBillList(BillsQuery query) {
      Assertion.Require(query, nameof(query));

      var filtering = query.MapToFilterString();
      var sorting = query.MapToSortString();

      FixedList<Bill> bills = BillData.GetBillList(filtering, sorting);

      return BillMapper.MapToBillListDto(bills);
    }


    public void SetBillAsPayed(string billUID) {

      BillData.SetBillAsPayed(billUID);
    }


    #endregion Use cases

    #region Private methods

    private void AssignConcepts(Bill bill) {

      bill.Concepts = BillConcept.GetListByBillId(bill.Id);

      foreach (var concept in bill.Concepts) {

        concept.TaxEntries = BillTaxEntry.GetListByBillConceptId(concept.Id);
      }
    }


    private Bill CreateBillByCategory(IPayable payable, BillFields fields, BillCategory billCategory) {

      var bill = new Bill(payable, billCategory, fields.BillNo);

      bill.Update(fields);

      bill.Save();

      bill.Concepts = CreateBillConcepts(bill, fields.Concepts);

      return bill;
    }


    private FixedList<BillConcept> CreateBillConcepts(Bill bill,
                                                      FixedList<BillConceptFields> conceptFields) {
      var concepts = new List<BillConcept>();

      foreach (BillConceptFields fields in conceptFields) {

        var billConcept = new BillConcept(bill, Product.Empty);

        billConcept.Update(fields);

        billConcept.Save();

        billConcept.TaxEntries = CreateBillTaxEntries(bill, billConcept, fields.TaxEntries);

        concepts.Add(billConcept);
      }

      return concepts.ToFixedList();
    }


    private Bill CreateBillImplementation(IPayable payable, BillFields fields) {

      return CreateBillByCategory(payable, fields, BillCategory.Factura);
    }

    
    private FixedList<BillTaxEntry> CreateBillTaxEntries(Bill bill,
                                                         BillConcept billConcept,
                                                         FixedList<BillTaxEntryFields> allTaxesFields) {

      var taxesList = new List<BillTaxEntry>(allTaxesFields.Count);

      foreach (BillTaxEntryFields taxFields in allTaxesFields) {

        var billTaxEntry = new BillTaxEntry(bill, billConcept);

        billTaxEntry.Update(taxFields);

        billTaxEntry.Save();

        taxesList.Add(billTaxEntry);
      }

      return taxesList.ToFixedList();
    }


    private Bill CreateBillTest(BillFields fields) {

      var billCategory = BillCategory.NotaDeCredito;

      var bill = new Bill(billCategory, fields.BillNo);

      bill.Update(fields);

      bill.Save();

      bill.Concepts = CreateBillConcepts(bill, fields.Concepts);

      return bill;
    }


    private Bill CreateCreditNoteImplementation(IPayable payable, BillFields fields) {

      return CreateBillByCategory(payable, fields, BillCategory.NotaDeCredito);
    }


    #endregion Private methods

  } // class BillUseCases

} // namespace Empiria.Billing.UseCases
