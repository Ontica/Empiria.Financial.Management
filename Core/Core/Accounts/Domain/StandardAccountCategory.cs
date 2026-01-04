/* Empiria Financial  ****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder                      *
*  Type     : StandardAccountCategory                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents an standard account category.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial.Data;

namespace Empiria.Financial {

  /// <summary>Represents an standard account category.</summary>
  public class StandardAccountCategory : CommonStorage {

    #region Constructors and parsers

    protected StandardAccountCategory() {
      // Required by Empiria Framework
    }

    static public StandardAccountCategory Parse(int id) => ParseId<StandardAccountCategory>(id);

    static public StandardAccountCategory Parse(string uid) => ParseKey<StandardAccountCategory>(uid);

    static public StandardAccountCategory ParseWithNamedKey(string namedKey) =>
                                                        ParseNamedKey<StandardAccountCategory>(namedKey);

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


    public new string NamedKey {
      get {
        return base.NamedKey;
      }
      private set {
        base.NamedKey = value;
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


    public new FixedList<string> Roles {
      get {
        return base.Roles;
      }
    }

    public bool HasChild {
      get {
        return !Child.IsEmptyInstance;
      }
    }

    private StandardAccountCategory _child = null;
    public StandardAccountCategory Child {
      get {
        if (IsEmptyInstance) {
          return this;
        }

        if (_child != null) {
          return _child;
        }

        var child = GetList().Find(x => x.Parent.Equals(this));

        _child = child ?? Empty;

        return _child;
      }
    }

    #endregion Properties

    #region Methods

    public FixedList<StandardAccount> GetStandardAccounts() {
      return ChartOfAccounts.GetStandardAccounts(this);
    }


    public FixedList<StandardAccount> GetStandardAccounts(string keywords) {

      keywords = keywords ?? string.Empty;

      return StandardAccountDataService.GetStandardAccounts(this, keywords);
    }

    #endregion Methods

  } // class StandardAccountCategory

} // namespace Empiria.Financial
