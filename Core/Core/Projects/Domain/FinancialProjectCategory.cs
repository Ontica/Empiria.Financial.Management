/* Empiria Financial  ****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder                      *
*  Type     : FinancialProjectCategory                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents financial project category.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Projects {

  /// <summary>Represents financial project category.</summary>
  public class FinancialProjectCategory : CommonStorage {

    #region Constructors and parsers

    protected FinancialProjectCategory() {
      // Required by Empiria Framework
    }

    static public FinancialProjectCategory Parse(int id) => ParseId<FinancialProjectCategory>(id);

    static public FinancialProjectCategory Parse(string uid) => ParseKey<FinancialProjectCategory>(uid);

    static public FinancialProjectCategory Empty => ParseEmpty<FinancialProjectCategory>();

    static public FixedList<FinancialProjectCategory> GetList() {
      return GetStorageObjects<FinancialProjectCategory>();
    }

    #endregion Constructors and parsers

    public string SubprogramCode {
      get {
        return base.Code;
      }
    }

  } // class FinancialProjectCategory

} // namespace Empiria.Financial.Projects
