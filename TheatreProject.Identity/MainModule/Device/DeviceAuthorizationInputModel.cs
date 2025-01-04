// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using TheatreProject.Identity.MainModule.Consent;

namespace TheatreProject.Identity.MainModule.Device
{
    public class DeviceAuthorizationInputModel : ConsentInputModel
    {
        public string UserCode { get; set; }
    }
}