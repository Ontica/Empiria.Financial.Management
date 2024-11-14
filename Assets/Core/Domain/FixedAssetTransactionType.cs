/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Fixed Assets Management                    Component : Domain Layer                            *
*  Assembly : Empiria.Assets.Core.dll                    Pattern   : Information Holder                      *
*  Type     : FixedAssetTransactionType                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a fixed asset transaction type.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Assets {

  /// <summary>Represents a fixed asset transaction type.</summary>
  public class FixedAssetTransactionType : GeneralObject {

    #region Constructors and parsers

    static internal FixedAssetTransactionType Parse(int id) => ParseId<FixedAssetTransactionType>(id);

    static internal FixedAssetTransactionType Parse(string uid) => ParseKey<FixedAssetTransactionType>(uid);

    static internal FixedList<FixedAssetTransactionType> GetList() {
      return GetList<FixedAssetTransactionType>().ToFixedList();
    }

    static internal FixedAssetTransactionType Empty => ParseEmpty<FixedAssetTransactionType>();

    #endregion Constructors and parsers

  }  // class FixedAssetTransactionType

}  // namespace Empiria.Assets
