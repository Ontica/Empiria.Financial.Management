/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Information Holder                      *
*  Type     : SATUnidadMedida                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unidad de medida de acuerdo al catálogo del SAT México para facturación electrónica.           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Billing.SATCatalogues {

  /// <summary>Unidad de medida de acuerdo al catálogo del SAT México para facturación electrónica.</summary>
  public class SATUnidadMedida : GeneralObject {

    #region Constructors and parsers

    static public SATUnidadMedida Parse(int id) => ParseId<SATUnidadMedida>(id);

    static public SATUnidadMedida Parse(string uid) => ParseKey<SATUnidadMedida>(uid);

    static public FixedList<SATUnidadMedida> GetList() {
      return BaseObject.GetList<SATUnidadMedida>()
                       .ToFixedList();
    }

    static public SATUnidadMedida Empty => ParseEmpty<SATUnidadMedida>();

    #endregion Constructors and parsers

    #region Properties

    public string Clave {
      get {
        return ExtendedDataField.Get<string>("clave");
      }
      private set {
        ExtendedDataField.Set("clave", value);
      }
    }


    public override string Keywords {
      get {
        return EmpiriaString.BuildKeywords(Name, Clave);
      }
    }

    #endregion Properties

  } // class SATUnidadMedida

} // namespace Empiria.Billing.SATCatalogues
