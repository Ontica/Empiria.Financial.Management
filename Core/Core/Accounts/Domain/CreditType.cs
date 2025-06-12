/* Empiria Financial  ****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Common Storage Type                     *
*  Type     : CreditType                                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Describes a credit account type.                                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial {

  /// <summary>Describes a credit account type.</summary>
  public class CreditType : CommonStorage {

    #region Constructors and parsers

    static public CreditType Parse(int id) => ParseId<CreditType>(id);

    static public CreditType Parse(string uid) => ParseKey<CreditType>(uid);

    static public CreditType Empty => ParseEmpty<CreditType>();

    static public FixedList<CreditType> GetList() {
      return GetStorageObjects<CreditType>();
    }

    #endregion Constructors and parsers

  } // class CreditType

} // namespace Empiria.Financial
