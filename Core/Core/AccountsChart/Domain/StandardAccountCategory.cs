/* Empiria Financial  ******************************************************************************************
*                                                                                                            *
*  Module   : AccountsChart                              Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder                      *
*  Type     : StandardAccountCategory                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a standard account category.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial {

  /// <summary>Represents a standard account category.</summary>
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

    public StandardAccountsCatalogue Catalogue {
      get {
        if (this.IsEmptyInstance) {
          return StandardAccountsCatalogue.Empty;
        }
        return base.ExtData.Get<StandardAccountsCatalogue>("standardAccountsCatalogueId");
      }
      private set {
        if (this.IsEmptyInstance) {
          return;
        }
        base.ExtData.Set("standardAccountsCatalogueId", value.Id);
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

    #endregion Properties

    #region Methods

    public FixedList<StandardAccount> GetStandardAccounts() {
      return Catalogue.GetStandardAccounts(this);
    }

    #endregion Methods

  } // class StandardAccountCategory

} // namespace Empiria.Financial
