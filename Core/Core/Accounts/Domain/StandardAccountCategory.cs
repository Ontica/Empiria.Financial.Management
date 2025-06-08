/* Empiria Financial  ******************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder                      *
*  Type     : StandardAccountCategory                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an standard account category.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial {

  /// <summary>Represents an standard account category.</summary>
  public class StandardAccountCategory : CommonStorage {

    #region Constructors and parsers

    protected StandardAccountCategory() {
      // Required by Empiria Framework
    }

    static public StandardAccountCategory Parse(int id) => ParseId<StandardAccountCategory>(id);

    static public StandardAccountCategory Parse(string uid) => ParseKey<StandardAccountCategory>(uid);

    static public StandardAccountCategory Empty => ParseEmpty<StandardAccountCategory>();

    static public FixedList<StandardAccountCategory> GetList() {
      return GetStorageObjects<StandardAccountCategory>();
    }

    #endregion Constructors and parsers

    #region Properties

    public ChartOfAccounts ChartOfAccounts {
      get {
        if (this.IsEmptyInstance) {
          return ChartOfAccounts.Empty;
        }
        return base.ExtData.Get<ChartOfAccounts>("chartOfAccountsId");
      }
      private set {
        if (this.IsEmptyInstance) {
          return;
        }
        base.ExtData.Set("chartOfAccountsId", value.Id);
      }
    }


    public StandardAccountCategory Parent {
      get {
        return base.GetParent<StandardAccountCategory>();
      }
      private set {
        SetParent(value);
      }
    }


    public FixedList<string> Roles {
      get {
        return base.ExtData.GetFixedList<string>("roles", false);
      }
    }

    #endregion Properties

    #region Methods

    public FixedList<StandardAccount> GetStandardAccounts() {
      return ChartOfAccounts.GetStandardAccounts(this);
    }

    public bool PlaysRole(string role) {
      Assertion.Require(role, nameof(role));

      return Roles.Contains(role);
    }

    #endregion Methods

  } // class StandardAccountCategory

} // namespace Empiria.Financial
