/* Empiria Financial  ******************************************************************************************
*                                                                                                            *
*  Module   : AccountsChart                              Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder                      *
*  Type     : StandardAccountsCatalogue                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Defines an standard accounts catalogue.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Financial.Data;

namespace Empiria.Financial {

  /// <summary>Defines an standard accounts catalogue.</summary>
  public class StandardAccountsCatalogue : CommonStorage {

    #region Fields

    private Lazy<FixedList<StandardAccount>> _standardAccounts;

    #endregion Fields

    #region Constructors and parsers

    protected StandardAccountsCatalogue() {
      // Required by Empiria Framework
    }

    static public StandardAccountsCatalogue Parse(int id) => ParseId<StandardAccountsCatalogue>(id);

    static public StandardAccountsCatalogue Parse(string uid) => ParseKey<StandardAccountsCatalogue>(uid);

    static public StandardAccountsCatalogue Empty => ParseEmpty<StandardAccountsCatalogue>();

    static public FixedList<StandardAccountsCatalogue> GetList() {
      return GetStorageObjects<StandardAccountsCatalogue>();
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

  } // class StandardAccountsCatalogue

} // namespace Empiria.Financial
