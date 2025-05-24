/* Empiria Financial  ******************************************************************************************
*                                                                                                            *
*  Module   : AccountsChart                              Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder                      *
*  Type     : StandardAccountsCatalogue                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Defines an standard accounts catalogue.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial {

  /// <summary>Defines an standard accounts catalogue.</summary>
  public class StandardAccountsCatalogue : CommonStorage {

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

    #endregion Constructors and parsers

  } // class StandardAccountsCatalogue

} // namespace Empiria.Financial
