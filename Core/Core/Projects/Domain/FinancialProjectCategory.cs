/* Empiria Financial  ****************************************************************************************
*                                                                                                            *
*  Module   : Project                                    Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder                      *
*  Type     : FinancialProjectCategory                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents Project Category category.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial {

  /// <summary>Represents a standard account category.</summary>
  public class FinancialProjectCategory : CommonStorage {

    #region Constructors and parsers

    protected FinancialProjectCategory() {
      // Required by Empiria Framework
    }

    static public FinancialProjectCategory Parse(int id) => ParseId<FinancialProjectCategory>(id);

    static public FinancialProjectCategory Parse(string uid) => ParseKey<FinancialProjectCategory>(uid);

    static public FinancialProjectCategory Empty => ParseEmpty<FinancialProjectCategory>();

    static public FixedList<FinancialProjectCategory> GetList() {
      return GetList<FinancialProjectCategory>()
            .ToFixedList();
    }

    #endregion Constructors and parsers

    #region Properties

    #endregion Properties

    #region Methods

    #endregion Methods

  } // class FinancialProjectCategory

} // namespace Empiria.Financial
