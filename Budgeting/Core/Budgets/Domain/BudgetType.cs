﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budgets                                    Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Core.dll                 Pattern   : Power Type                              *
*  Type     : BudgetType                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes a budget.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Financial;
using Empiria.Ontology;

namespace Empiria.Budgeting {

  /// <summary>Power type that describes a budget.</summary>
  [Powertype(typeof(Budget))]
  public class BudgetType : Powertype {

    #region Constructors and parsers

    private BudgetType() {
      // Empiria power types always have this constructor.
    }

    static public new BudgetType Parse(int typeId) => Parse<BudgetType>(typeId);

    static public new BudgetType Parse(string typeName) => Parse<BudgetType>(typeName);

    static public FixedList<BudgetType> GetList() {
      return BaseBudgetType.ExtensionData.GetFixedList<BudgetType>("budgetTypes");
    }

    static private ObjectTypeInfo BaseBudgetType => Powertype.Parse("ObjectTypeInfo.PowerType.BudgetType");

    static public BudgetType Empty => Parse("ObjectTypeInfo.Budget.Empty");

    #endregion Constructors and parsers

    #region Properties

    public Currency Currency {
      get {
        return ExtensionData.Get("currencyId", Currency.Default);
      }
    }

    public bool Multiyear {
      get {
        return ExtensionData.Get("multiyear", false);
      }
    }

    public string Prefix {
      get {
        return ExtensionData.Get("prefix", string.Empty);
      }
    }


    public BudgetAccountSegmentType ProductProcurementSegmentType {
      get {
        return ExtensionData.Get("productProcurementSegmentTypeId", BudgetAccountSegmentType.Empty);
      }
    }


    public FixedList<BudgetAccountSegmentType> SegmentTypes {
      get {
        return base.ExtensionData.GetFixedList<BudgetAccountSegmentType>("segmentTypes");
      }
    }

    #endregion Properties

    #region Methods

    public FixedList<Budget> GetBudgets(DateTime fromDate, DateTime toDate) {
      return Budget.GetFullList<Budget>()
                   .FindAll(x => x.BudgetType.Equals(this) &&
                                 fromDate.Year <= x.Year && x.Year <= toDate.Year);
    }


    public FixedList<Budget> GetBudgets(DateTime fromDate, DateTime toDate,
                                        Predicate<Budget> predicate) {
      return Budget.GetFullList<Budget>()
                   .FindAll(x => x.BudgetType.Equals(this) &&
                                 fromDate.Year <= x.Year && x.Year <= toDate.Year &&
                                 predicate.Invoke(x));
    }


    public Budget GetCurrentBudget() {
      var budget = Budget.GetFullList<Budget>()
                         .Find(x => x.BudgetType.Equals(this) && x.Year == DateTime.Today.Year);

      Assertion.Require(budget, $"No hay presupuestos registrados para el año {DateTime.Today.Year}");

      return budget;
    }


    internal FixedList<NamedEntityDto> GetTransactionTypes() {
      FixedList<int> ids = base.ExtensionData.GetFixedList<int>("transactionTypes");

      return ids.Select(x => ObjectTypeInfo.Parse(x))
                .MapToNamedEntityList();
    }

    #endregion Methods

  }  // class BudgetType

}  // namespace Empiria.Budgeting
