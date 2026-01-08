/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Budget Transactions                        Component : Domain Layer                            *
*  Assembly : Empiria.Budgeting.Transactions.dll         Pattern   : Common Storage Type                     *
*  Type     : BalanceColumn                              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a column balance that holds deposits or withdrawals for a budget entry.             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Budgeting.Transactions {

  /// <summary>Represents a column balance that holds deposits or withdrawals for a budget entry.</summary>
  public class BalanceColumn : CommonStorage, INamedEntity {

    #region Constructors and parsers

    static public BalanceColumn Parse(int id) => ParseId<BalanceColumn>(id);

    static public BalanceColumn Parse(string uid) => ParseKey<BalanceColumn>(uid);

    static public BalanceColumn Empty => ParseEmpty<BalanceColumn>();

    static public BalanceColumn Planned => ParseNamedKey<BalanceColumn>("Planned");

    static public BalanceColumn Authorized => ParseNamedKey<BalanceColumn>("Authorized");

    static public BalanceColumn Expanded => ParseNamedKey<BalanceColumn>("Expanded");

    static public BalanceColumn Reduced => ParseNamedKey<BalanceColumn>("Reduced");

    static public BalanceColumn Modified => ParseNamedKey<BalanceColumn>("Modified");

    static public BalanceColumn Requested => ParseNamedKey<BalanceColumn>("Requested");

    static public BalanceColumn Commited => ParseNamedKey<BalanceColumn>("Commited");

    static public BalanceColumn ToPay => ParseNamedKey<BalanceColumn>("ToPay");

    static public BalanceColumn Exercised => ParseNamedKey<BalanceColumn>("Exercised");

    static public BalanceColumn Available => ParseNamedKey<BalanceColumn>("Available");

    #endregion Constructors and parsers

    #region Properties

    string INamedEntity.Name {
      get {
        return OperationVerb;
      }
    }

    public string OperationVerb {
      get {
        return ExtData.Get("operationVerb", base.Name);
      }
    }

    #endregion Properties

  }  // class BalanceColumn

}  // namespace Empiria.Budgeting.Transactions
