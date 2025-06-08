/* Empiria Financial  ******************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder                      *
*  Type     : ChartOfAccounts                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Defines a chart of accounts that is an aggregate of standard accounts.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Financial.Data;

namespace Empiria.Financial {

  /// <summary>Defines a chart of accounts that is an aggregate of standard accounts.</summary>
  public class ChartOfAccounts : CommonStorage {

    #region Fields

    private Lazy<FixedList<StandardAccount>> _standardAccounts;

    #endregion Fields

    #region Constructors and parsers

    protected ChartOfAccounts() {
      // Required by Empiria Framework
    }

    static public ChartOfAccounts Parse(int id) => ParseId<ChartOfAccounts>(id);

    static public ChartOfAccounts Parse(string uid) => ParseKey<ChartOfAccounts>(uid);

    static public ChartOfAccounts Empty => ParseEmpty<ChartOfAccounts>();

    static public FixedList<ChartOfAccounts> GetList() {
      return GetStorageObjects<ChartOfAccounts>();
    }

    protected override void OnLoad() {
      _standardAccounts = new Lazy<FixedList<StandardAccount>>(() => StandardAccountDataService.GetStandardAccounts(this));
    }

    #endregion Constructors and parsers

    public FixedList<StandardAccount> GetStandardAccounts() {
      return _standardAccounts.Value;
    }


    public FixedList<StandardAccount> GetStandardAccounts(StandardAccountCategory category) {
      Assertion.Require(category, nameof(category));

      return _standardAccounts.Value.FindAll(x => x.Category.Equals(category));
    }

  } // class ChartOfAccounts

} // namespace Empiria.Financial
