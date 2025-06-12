/* Empiria Financial  ****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Common Storage Type                     *
*  Type     : CreditStage                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes a credit account stage.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial {

  /// <summary>Describes a credit account stage.</summary>
  public class CreditStage : CommonStorage {

    #region Constructors and parsers

    static public CreditStage Parse(int id) => ParseId<CreditStage>(id);

    static public CreditStage Parse(string uid) => ParseKey<CreditStage>(uid);

    static public CreditStage Empty => ParseEmpty<CreditStage>();

    static public FixedList<CreditStage> GetList() {
      return GetStorageObjects<CreditStage>();
    }

    #endregion Constructors and parsers

  } // class CreditStage

} // namespace Empiria.Financial
