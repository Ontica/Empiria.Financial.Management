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

using Empiria.Ontology;

namespace Empiria.Financial {

  /// <summary>Interface for budgetable entities. Budgetable entities can be product orders
  /// or requisitions, loans, payment liabilities, or any other operational or financial
  /// transaction that affects a budget.</summary>
  public interface IBudgetable : IIdentifiable, INamedEntity {

    FixedList<INamedEntity> BudgetTransactions {
      get;
    }

    FixedList<ITaxEntry> Taxes {
      get;
    }

    ObjectTypeInfo GetEmpiriaType();

    FixedList<IPayableEntity> GetPayableEntities();

  } // interface IBudgetable



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
