/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Domain Layer                            *
*  Assembly : Empiria.Assets.Core.dll                    Pattern   : Information Holder                      *
*  Type     : FixedAssetType                             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a fixed asset type.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Assets {

  /// <summary>Represents a fixed asset type.</summary>
  public class FixedAssetType : GeneralObject {

    #region Constructors and parsers

    static internal FixedAssetType Parse(int id) {
      return ParseId<FixedAssetType>(id);
    }

    static internal FixedAssetType Parse(string uid) {
      return ParseKey<FixedAssetType>(uid);
    }

    static internal FixedList<FixedAssetType> GetList() {
      return GetList<FixedAssetType>().ToFixedList();
    }

    static internal FixedAssetType Empty => ParseEmpty<FixedAssetType>();

    #endregion Constructors and parsers

  }  // class FixedAssetType

}  // namespace Empiria.Assets
