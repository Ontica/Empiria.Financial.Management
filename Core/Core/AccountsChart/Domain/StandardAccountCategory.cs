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
      return GetList<StandardAccountCategory>()
            .ToFixedList();
    }

    #endregion Constructors and parsers

    #region Properties

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

    #endregion Methods

  } // class StandardAccountCategory

} // namespace Empiria.Financial
