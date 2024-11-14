/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Use cases Layer                         *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Use case interactor class               *
*  Type     : BillUseCases                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Use cases to manage billing.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;
using Empiria.Billing.Adapters;
using Empiria.Billing.Data;
using Empiria.Billing.SATMexicoImporter;
using Empiria.Financial;
using Empiria.Products;
using Empiria.Services;

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


    public BillDto CreateBill(string xmlFilePath, IPayableEntity iPayableEntity) {
      Assertion.Require(xmlFilePath, nameof(xmlFilePath));
      Assertion.Require(iPayableEntity, nameof(iPayableEntity));

      var reader = new SATBillXmlReader(xmlFilePath);

      SATBillDto satDto = reader.ReadAsBillDto();

      BillFields fields = BillFieldsMapper.Map(satDto);
      fields.PayableId = iPayableEntity.Id;

      Bill bill = CreateBill(fields);

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


    private Bill CreateBill(BillFields fields) {

      var billCategory = BillCategory.Factura;

      var bill = new Bill(billCategory, fields.BillNo);

      bill.Update(fields);

      bill.Save();

      FixedList<BillConcept> concepts = CreateBillConcepts(bill, fields.Concepts);

      bill.Concepts = concepts;

      return bill;
    }


    private FixedList<BillConcept> CreateBillConcepts(Bill bill, FixedList<BillConceptFields> conceptFields) {

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

    


    #endregion Private methods


  } // class BillUseCases

} // namespace Empiria.Billing.UseCases
