/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Common Storage Type                     *
*  Type     : BudgetSpecialOperationType                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a budget special transaction operation type.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Transactions {

  /// <summary>Represents a budget special transaction operation type.</summary>
  public class BudgetSpecialOperationType : CommonStorage, INamedEntity {

    #region Constructors and parsers

    static public BudgetSpecialOperationType Parse(int id) =>
          ParseId<BudgetSpecialOperationType>(id);

    static public BudgetSpecialOperationType Parse(string uid) =>
          ParseKey<BudgetSpecialOperationType>(uid);

    static public BudgetSpecialOperationType Empty =>
          ParseEmpty<BudgetSpecialOperationType>();

    static public FixedList<BudgetSpecialOperationType> GetList() =>
      GetStorageObjects<BudgetSpecialOperationType>();

    #endregion Constructors and parsers

  }  // class BudgetSpecialOperationType

}  // namespace Empiria.Budgeting.Transactions
