/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Integration Interface                   *
*  Type     : IBudgetable                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Interface for budgetable entities. Budgetable entities can be product orders                   *
*             or requisitions, loans, payment liabilities, or any other operational or financial             *
*             transaction that affects a budget.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial;

using Empiria.Ontology;

namespace Empiria.Budgeting.Transactions {

  /// <summary>Interface for budgetable entities. Budgetable entities can be product orders
  /// or requisitions, loans, payment liabilities, or any other operational or financial
  /// transaction that affects a budget.</summary>
  public interface IBudgetable : IIdentifiable, INamedEntity {


    FixedList<ITaxEntry> Taxes {
      get;
    }

    ObjectTypeInfo GetEmpiriaType();


  } // interface IBudgetable


  public interface ITaxEntry {

    TaxType TaxType {
      get;
    }

    decimal Total {
      get;
    }

  }  // interface ITaxEntry

} // namespace Empiria.Budgeting.Transactions
