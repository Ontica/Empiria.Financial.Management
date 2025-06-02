/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Structurer                              *
*  Type     : CashFlowProjectionByYear                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a cash flow projection with its entries and amounts grouped for a whole year.       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Linq;

using Empiria.Financial;
using Empiria.Products;

namespace Empiria.CashFlow.Projections {

  /// <summary>Represents a cash flow projection with its entries and amounts grouped for a whole year.</summary>
  public class CashFlowProjectionByYear {

    #region Constructors and parsers

    public CashFlowProjectionByYear(CashFlowProjection projection) {
      Assertion.Require(projection, nameof(projection));
      Assertion.Require(!projection.IsEmptyInstance, "Transaction can't be the empty instance.");
      Assertion.Require(!projection.IsNew, "Transaction can't be a new instance.");

      Projection = projection;
    }

    #endregion Constructors and parsers

    #region Properties

    public CashFlowProjection Projection {
      get;
    }

    #endregion Properties

    #region Methods

    internal string BuildUID(CashFlowProjectionEntry entry) {
      Assertion.Require(entry, nameof(entry));

      return $"{entry.Projection.Id}|{entry.ProjectionColumn.Id}|{entry.CashFlowAccount.Id}|" +
             $"{entry.Product.Id}|{entry.ProductUnit.Id}|{entry.Currency.Id}|{entry.Year}";
    }


    static internal string BuildUID(CashFlowProjectionEntryByYear entry) {
      Assertion.Require(entry, nameof(entry));

      return $"{entry.Projection.Id}|{entry.ProjectionColumn.Id}|{entry.CashFlowAccount.Id}|" +
             $"{entry.Product.Id}|{entry.ProductUnit.Id}|{entry.Currency.Id}|{entry.Year}";
    }


    static internal CashFlowProjectionEntryByYearFields BuildFields(string entryByYearUID) {
      Assertion.Require(entryByYearUID, nameof(entryByYearUID));

      int[] parts = EmpiriaString.SplitToIntArray(entryByYearUID, '|');

      return new CashFlowProjectionEntryByYearFields {
         UID = entryByYearUID,
         ProjectionUID = CashFlowProjection.Parse(parts[0]).UID,
         ProjectionColumnUID = CashFlowProjectionColumn.Parse(parts[1]).UID,
         CashFlowAccountUID = FinancialAccount.Parse(parts[2]).UID,
         ProductUID = Product.Parse(parts[3]).UID,
         ProductUnitUID = ProductUnit.Parse(parts[4]).UID,
         CurrencyUID = Currency.Parse(parts[5]).UID,
         Year = parts[6],
      };
    }


    internal FixedList<CashFlowProjectionEntry> CreateProjectionEntries(CashFlowProjectionEntryByYearFields fields) {
      Assertion.Require(fields, nameof(fields));

      FixedList<CashFlowProjectionEntry> currentEntries = GetProjectionEntries(fields);

      if (currentEntries.Count > 0) {
        Assertion.RequireFail(AlreadyExistsMsg(currentEntries[0]));
      }

      return GetNewEntries(fields).ToFixedList();
    }


    internal FixedList<CashFlowProjectionEntry> GetProjectionEntries(string entryByYearUID) {
      Assertion.Require(entryByYearUID, nameof(entryByYearUID));

      int[] parts = EmpiriaString.SplitToIntArray(entryByYearUID, '|');

      Assertion.Ensure(parts[0] == Projection.Id, "Cash flow projection Id mismatch.");

      FixedList<CashFlowProjectionEntry> entries = Projection.Entries.FindAll(x =>
                                                                x.ProjectionColumn.Id == parts[1] &&
                                                                x.CashFlowAccount.Id == parts[2] &&
                                                                x.Product.Id == parts[3] &&
                                                                x.ProductUnit.Id == parts[4] &&
                                                                x.Currency.Id == parts[5] &&
                                                                x.Year == parts[6]
                                                              );

      return entries.Sort((x, y) => x.Month.CompareTo(y.Month));
    }


    internal FixedList<CashFlowProjectionEntry> GetProjectionEntries(CashFlowProjectionEntryByYearFields fields) {
      Assertion.Require(fields, nameof(fields));

      var column = FieldPatcher.PatchField(fields.ProjectionColumnUID, CashFlowProjectionColumn.Empty);
      var account = FieldPatcher.PatchField(fields.CashFlowAccountUID, FinancialAccount.Empty);
      var product = FieldPatcher.PatchField(fields.ProductUID, Product.Empty);
      var productUnit = FieldPatcher.PatchField(fields.ProductUnitUID, ProductUnit.Empty);
      var currency = FieldPatcher.PatchField(fields.CurrencyUID, Projection.Plan.BaseCurrency);
      var year = fields.Year;

      FixedList<CashFlowProjectionEntry> entries = Projection.Entries.FindAll(x =>
                                                                x.ProjectionColumn.Equals(column) &&
                                                                x.CashFlowAccount.Equals(account) &&
                                                                x.Product.Equals(product) &&
                                                                x.ProductUnit.Equals(productUnit) &&
                                                                x.Currency.Equals(currency) &&
                                                                x.Year == year
                                                              );

      return entries.Sort((x, y) => x.Month.CompareTo(y.Month));
    }


    public FixedList<CashFlowProjectionEntryByYear> GetEntries() {
      var groups = Projection.Entries.GroupBy(x => BuildUID(x));

      var list = new List<CashFlowProjectionEntryByYear>(groups.Count());

      foreach (var group in groups) {
        var item = new CashFlowProjectionEntryByYear(group.Key, group.ToFixedList());

        list.Add(item);
      }

      return list.ToFixedList()
                 .Sort((x, y) => x.CashFlowAccount.Name.CompareTo(y.CashFlowAccount.Name));
    }


    internal FixedList<CashFlowProjectionEntry> GetUpdatedEntries(CashFlowProjectionEntryByYearFields fields) {
      Assertion.Require(fields, nameof(fields));

      var updatedBudgetEntries = new List<CashFlowProjectionEntry>(12);

      updatedBudgetEntries.AddRange(GetCurrentEntriesToDelete(fields));
      updatedBudgetEntries.AddRange(GetNewEntries(fields));
      updatedBudgetEntries.AddRange(GetChangedProjectionEntries(fields));
      updatedBudgetEntries.AddRange(GetDeletedEntries(fields));

      return updatedBudgetEntries.ToFixedList();
    }

    #endregion Methods

    #region Helpers

    private string AlreadyExistsMsg(CashFlowProjectionEntry entry) {
      var msg = $"La proyección ya contiene un movimiento con el concepto " +
                $"[{entry.ProjectionColumn.Name} - {entry.Year}] {entry.ProjectionColumn.Name}.";

      if (!entry.Product.IsEmptyInstance) {
        msg += $" Producto: {entry.Product.Name}.";
      }

      return msg;
    }


    private FixedList<CashFlowProjectionEntry> GetChangedProjectionEntries(CashFlowProjectionEntryByYearFields fields) {
      var list = new List<CashFlowProjectionEntry>(12);

      var currentEntries = GetProjectionEntries(fields);

      FixedList<CashFlowProjectionMonthEntryFields> amounts = fields.Amounts.ToFixedList();

      var changedEntries = amounts.FindAll(x => x.Amount != 0 && x.CashFlowEntryUID.Length != 0 &&
                                                currentEntries.Contains(y => y.UID == x.CashFlowEntryUID));

      foreach (var amount in changedEntries) {
        CashFlowProjectionEntryFields entryFields = TransformToCashFlowProjectionEntryFields(fields, amount);

        var entry = currentEntries.Find(x => x.UID == amount.CashFlowEntryUID);

        entry.Update(entryFields);

        list.Add(entry);
      }

      return list.ToFixedList();
    }


    private FixedList<CashFlowProjectionEntry> GetCurrentEntriesToDelete(CashFlowProjectionEntryByYearFields fields) {
      var currentEntries = GetProjectionEntries(fields);

      if (currentEntries.Count() != 0 && BuildUID(currentEntries[0]) == fields.UID) {
        return new FixedList<CashFlowProjectionEntry>();
      } else if (currentEntries.Count() != 0 && BuildUID(currentEntries[0]) != fields.UID) {
        Assertion.RequireFail(AlreadyExistsMsg(currentEntries[0]));
      }

      FixedList<CashFlowProjectionEntry> toDeleteEntries = GetProjectionEntries(fields.UID);

      foreach (var entry in toDeleteEntries) {
        entry.Delete();
      }

      return toDeleteEntries;
    }


    private FixedList<CashFlowProjectionEntry> GetDeletedEntries(CashFlowProjectionEntryByYearFields fields) {
      var list = new List<CashFlowProjectionEntry>(12);

      var currentEntries = GetProjectionEntries(fields);

      FixedList<CashFlowProjectionMonthEntryFields> amounts = fields.Amounts.ToFixedList();

      var deletedEntries = currentEntries.FindAll(x => !amounts.Contains(y => x.Month == y.Month));

      foreach (var entry in deletedEntries) {
        entry.Delete();

        list.Add(entry);
      }

      return list.ToFixedList();
    }


    private FixedList<CashFlowProjectionEntry> GetNewEntries(CashFlowProjectionEntryByYearFields fields) {
      var list = new List<CashFlowProjectionEntry>(12);

      var currentEntries = GetProjectionEntries(fields);

      FixedList<CashFlowProjectionMonthEntryFields> amounts = fields.Amounts.ToFixedList();

      var newEntries = amounts.FindAll(x => x.Amount != 0 &&
                                            !currentEntries.Contains(y => y.UID == x.CashFlowEntryUID));

      var projectionColumn = CashFlowProjectionColumn.Parse(fields.ProjectionColumnUID);
      var cashFlowAccount = FinancialAccount.Parse(fields.CashFlowAccountUID);

      foreach (var amount in newEntries) {

        CashFlowProjectionEntryFields entryFields = TransformToCashFlowProjectionEntryFields(fields, amount);

        var entry = new CashFlowProjectionEntry(Projection, projectionColumn, cashFlowAccount,
                                                fields.Year, amount.Month, amount.Amount);

        entry.Update(entryFields);

        list.Add(entry);
      }

      return list.ToFixedList();
    }


    private CashFlowProjectionEntryFields TransformToCashFlowProjectionEntryFields(CashFlowProjectionEntryByYearFields fields,
                                                                                   CashFlowProjectionMonthEntryFields amount) {
      return new CashFlowProjectionEntryFields {
        ProjectionUID = fields.ProjectionUID,
        ProjectionColumnUID = fields.ProjectionColumnUID,
        CashFlowAccountUID = fields.CashFlowAccountUID,
        CurrencyUID = fields.CurrencyUID,
        ProductUID = fields.ProductUID,
        ProductUnitUID = fields.ProductUnitUID,
        Description = fields.Description,
        Justification = fields.Justification,
        Year = fields.Year,
        Month = amount.Month,
        Amount = amount.Amount,
        ProductQty = amount.ProductQty,
      };
    }

    #endregion Helpers

  }  // class CashFlowProjectionByYear

}  //namespace Empiria.CashFlow.Projections
