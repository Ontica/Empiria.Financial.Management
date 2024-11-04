﻿/* Empiria Financial *****************************************************************************************
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

    static public BalanceColumn Planned => Parse("Planned");

    static public BalanceColumn Authorized => Parse("Authorized");

    static public BalanceColumn Expanded => Parse("Expanded");

    static public BalanceColumn Reduced => Parse("Reduced");

    static public BalanceColumn Modified => Parse("Modified");

    static public BalanceColumn Available => Parse("Available");

    static public BalanceColumn Commited => Parse("Commited");

    static public BalanceColumn ToPay => Parse("ToPay");

    static public BalanceColumn Exercised => Parse("Exercised");

  }  // class BalanceColumn

}  // namespace Empiria.Budgeting.Transactions
