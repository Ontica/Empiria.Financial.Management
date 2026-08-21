/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Adapters Layer                          *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Output DTO                              *
*  Type     : BillsStructureDto                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return bill type information.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents;

namespace Empiria.Billing.Adapters {

  /// <summary>Data transfer object used to return bill type information.</summary>
  public class BillTypeDto {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string FileType {
      get; internal set;
    }

    public string ApplicationContentType {
      get; internal set;
    }

    public bool IsCFDI {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    #region Helpers

    static public BillTypeDto MapToBillTypeDto(DocumentProduct document) {
      return new BillTypeDto {
        UID = document.UID,
        Name = document.Name,
        FileType = document.FileType.ToString(),
        ApplicationContentType = document.ApplicationContentType,
        IsCFDI = document.Attributes.Get("isCFDI", false),
        Description = document.Description
      };
    }

    #endregion Helpers

  } // class BillTypeDto

} // namespace Empiria.Billing.Adapters
