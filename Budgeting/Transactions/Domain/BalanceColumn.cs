/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Information Holder                      *
*  Type     : BalanceColumn                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a column balance that holds deposits or withdrawals in a budget entry.              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Transactions {

  public class BalanceColumn : GeneralObject {

    static public BalanceColumn Parse(int id) => ParseId<BalanceColumn>(id);

    static public BalanceColumn Parse(string uid) => ParseKey<BalanceColumn>(uid);

    static public BalanceColumn Empty => ParseEmpty<BalanceColumn>();

  }  // class BalanceColumn

}  // namespace Empiria.Budgeting.Transactions
