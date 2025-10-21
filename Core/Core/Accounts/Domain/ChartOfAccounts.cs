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

using Empiria.Financial.Concepts;

using Empiria.Financial.Data;

namespace Empiria.Financial {

  /// <summary>Defines an automatic grouping of standard account categories under a financial concept group.</summary>
  public class AutoGrouping {

    internal AutoGrouping(FinancialConceptGroup financialConceptGroup,
                          FixedList<StandardAccountCategory> standardAccountCategories) {
      Assertion.Require(financialConceptGroup, nameof(financialConceptGroup));
      Assertion.Require(standardAccountCategories, nameof(standardAccountCategories));

      FinancialConceptGroup = financialConceptGroup;
      StandardAccountCategories = standardAccountCategories;
    }

    static public AutoGrouping Empty = new AutoGrouping(FinancialConceptGroup.Empty,
                                                        FixedList<StandardAccountCategory>.Empty);

    #region Properties

    internal bool IsEmptyInstance {
      get {
        return FinancialConceptGroup.IsEmptyInstance;
      }
    }


    public FinancialConceptGroup FinancialConceptGroup {
      get;
    }


    public FixedList<StandardAccountCategory> StandardAccountCategories {
      get;
    }

    #endregion Properties

  }  // class AutoGrouping



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

    #region Properties

    public AutoGrouping AutoGrouping {
      get {
        if (!ExtData.Contains("autoGrouping")) {
          return AutoGrouping.Empty;
        }

        return new AutoGrouping(ExtData.Get<FinancialConceptGroup>("autoGrouping/groupId"),
                                ExtData.GetFixedList<StandardAccountCategory>("autoGrouping/stdAccountCategories"));
      }
    }

    #endregion Properties

    #region Methods

    internal StandardAccount GetStandardAccount(string stdAccountUID) {
      Assertion.Require(stdAccountUID, nameof(stdAccountUID));

      StandardAccount stdAccount = _standardAccounts.Value.Find(x => x.UID == stdAccountUID);

      Assertion.Require(stdAccount,
        $"An standard account with uid {stdAccountUID} does not exists in this chart of accounts.");

      return stdAccount;
    }


    public FixedList<StandardAccount> GetStandardAccounts() {
      return _standardAccounts.Value;
    }


    public FixedList<StandardAccount> GetStandardAccounts(StandardAccountCategory category) {
      Assertion.Require(category, nameof(category));

      return _standardAccounts.Value.FindAll(x => x.Category.Equals(category));
    }

    #endregion Methods

  } // class ChartOfAccounts

} // namespace Empiria.Financial
