/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Power type                              *
*  Type     : BudgetTransactionType                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes a budget transaction.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Ontology;

namespace Empiria.Budgeting.Transactions {

  /// <summary>Power type that describes a budget transaction.</summary>
  [Powertype(typeof(BudgetTransaction))]
  public sealed class BudgetTransactionType : Powertype {

    #region Constructors and parsers

    private BudgetTransactionType() {
      // Empiria powertype types always have this constructor.
    }

    static public new BudgetTransactionType Parse(int typeId) => Parse<BudgetTransactionType>(typeId);

    static public new BudgetTransactionType Parse(string typeName) => Parse<BudgetTransactionType>(typeName);

    #endregion Constructors and parsers

  } // class BudgetTransactionType

}  // namespace Empiria.Budgeting.Transactions
