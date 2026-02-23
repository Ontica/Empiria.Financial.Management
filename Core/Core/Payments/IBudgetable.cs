/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial                                  Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Integration interface                   *
*  Type     : IBudgetable                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Interface for budgetable entities. Budgetable entities can be product orders                   *
*             or requisitions, loans, payment liabilities, or any other operational or financial             *
*             transaction that affects a budget.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Locations;
using Empiria.Ontology;
using Empiria.Parties;
using Empiria.Products;
using Empiria.Projects;

namespace Empiria.Financial {

  /// <summary>Interface for budgetable entities. Budgetable entities can be product orders
  /// or requisitions, loans, payment liabilities, or any other operational or financial
  /// transaction that affects a budget.</summary>
  public interface IBudgetable : IIdentifiable, INamedEntity {

    BudgetableData Data {
      get;
    }

    FixedList<BudgetableItemData> Items {
      get;
    }

    FixedList<ITaxEntry> Taxes {
      get;
    }

    FixedList<IPayableEntity> GetPayableEntities();

  } // interface IBudgetable



  /// <summary>Holds data for budgetable entity. A budgetable entity can be an order
  /// or requisition, a loan or payment liability.</summary>
  public class BudgetableData {

    public ObjectTypeInfo BudgetableType {
      get; set;
    }

    public IIdentifiable BaseBudget {
      get; set;
    }

    public Party RequestedBy {
      get; set;
    } = Party.Empty;


    public Currency Currency {
      get; set;
    } = Currency.Default;


    public decimal ExchangeRate {
      get; set;
    } = decimal.One;


    public string BudgetableNo {
      get; set;
    } = string.Empty;


    public string Justification {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string Keywords {
      get; set;
    } = string.Empty;


    public void EnsureValid() {
      Assertion.Require(BudgetableType, nameof(BudgetableType));
      Assertion.Require(BudgetableNo, nameof(BudgetableNo));
      Assertion.Require(BaseBudget, nameof(BaseBudget));
      Assertion.Require(RequestedBy, nameof(RequestedBy));
      Assertion.Require(Currency, nameof(Currency));
      Assertion.Require(ExchangeRate > decimal.Zero, $"{nameof(ExchangeRate)} must be positive.");
      Assertion.Require(Justification != null, nameof(Justification));
      Assertion.Require(Description != null, nameof(Description));
      Assertion.Require(Keywords, nameof(Keywords));

      if (Currency.Equals(Currency.Default)) {
        Assertion.Require(ExchangeRate == decimal.One,
          $"{nameof(ExchangeRate)} must be equal to $1.00 when currency is default.");
      } else {
        Assertion.Require(ExchangeRate != decimal.One,
          $"{nameof(ExchangeRate)} must be distinct to $1.00 when currency is not default.");
      }
    }

  }  // class BudgetableData



  /// <summary>Holds data for budgetable items. A budgetable item represents a single line item
  /// in a budgetable entity, such as a product or service in an order or requisition,
  /// or a payment liability in a loan or payable order.</summary>
  public class BudgetableItemData {

    public BaseObject BudgetableItem {
      get; set;
    }

    public IIdentifiable BudgetEntry {
      get; set;
    }

    public IIdentifiable Budget {
      get; set;
    }

    public FinancialAccount BudgetAccount {
      get; set;
    } = FinancialAccount.Empty;


    public Product Product {
      get; set;
    } = Product.Empty;


    public string ProductCode {
      get; set;
    } = string.Empty;


    public string ProductName {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string Justification {
      get; set;
    } = string.Empty;


    public ProductUnit ProductUnit {
      get; set;
    } = ProductUnit.Empty;


    public Country OriginCountry {
      get; set;
    } = Country.Default;


    public decimal ProductQty {
      get; set;
    }

    public Project Project {
      get; set;
    } = Project.Empty;


    public Party Party {
      get; set;
    } = Party.Empty;


    public DateTime BudgetingDate {
      get; set;
    } = DateTime.Today.Date;


    public Currency Currency {
      get; set;
    } = Currency.Default;


    public decimal ExchangeRate {
      get; set;
    } = decimal.One;


    public decimal CurrencyAmount {
      get; set;
    }


    public decimal Amount {
      get {
        return Math.Round(CurrencyAmount * ExchangeRate, 2);
      }
    }


    public bool HasRelatedBudgetEntry {
      get {
        return RelatedBudgetEntry != null;
      }
    }

    public IIdentifiable RelatedBudgetEntry {
      get; set;
    }

    public void EnsureValid() {
      Assertion.Require(BudgetableItem, nameof(BudgetableItem));
      Assertion.Require(BudgetEntry, nameof(BudgetEntry));
      Assertion.Require(Budget, nameof(Budget));
      Assertion.Require(BudgetAccount != FinancialAccount.Empty, nameof(BudgetAccount));
      Assertion.Require(Product, nameof(Product));
      Assertion.Require(ProductCode != null, nameof(ProductCode));
      Assertion.Require(ProductName != null, nameof(ProductName));
      Assertion.Require(ProductUnit, nameof(ProductUnit));
      Assertion.Require(OriginCountry, nameof(OriginCountry));
      Assertion.Require(Description != null, nameof(Description));
      Assertion.Require(Justification != null, nameof(Justification));
      Assertion.Require(ProductQty > 0, $"{nameof(ProductQty)} must be greater than zero.");
      Assertion.Require(Project, nameof(Project));
      Assertion.Require(Party, nameof(Party));
      Assertion.Require(!ExecutionServer.IsMinOrMaxDate(BudgetingDate), $"Invalid {nameof(BudgetingDate)}.");
      Assertion.Require(BudgetingDate.Year >= DateTime.Today.Year, $"Invalid {nameof(BudgetingDate)}.");
      Assertion.Require(Currency, nameof(Currency));
      Assertion.Require(ExchangeRate > decimal.Zero, $"{nameof(ExchangeRate)} must be positive.");
      Assertion.Require(CurrencyAmount > decimal.Zero, $"{nameof(CurrencyAmount)} must be positive.");

      if (Currency.Equals(Currency.Default)) {
        Assertion.Require(ExchangeRate == decimal.One,
          $"{nameof(ExchangeRate)} must be equal to $1.00 when currency is default.");
      } else {
        Assertion.Require(ExchangeRate != decimal.One,
          $"{nameof(ExchangeRate)} must be distinct to $1.00 when currency is not default.");
      }
    }

  }  // class BudgetableItemData


  /// <summary>Represents a tax entry for budgetable entities.</summary>
  public interface ITaxEntry {

    TaxType TaxType {
      get;
    }

    decimal Total {
      get;
    }

  }  // interface ITaxEntry

} // namespace Empiria.Financial
