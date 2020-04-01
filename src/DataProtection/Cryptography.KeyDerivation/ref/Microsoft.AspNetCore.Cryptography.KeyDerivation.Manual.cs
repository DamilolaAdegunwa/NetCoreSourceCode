// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Cryptography.KeyDerivation.PBKDF2
{
    internal partial interface IPbkdf2Provider
    {
        byte[] DeriveKey(string password, byte[] salt, Microsoft.AspNetCore.Cryptography.KeyDerivation.KeyDerivationPrf prf, int iterationCount, int numBytesRequested);
    }

    internal sealed partial class ManagedPbkdf2Provider : Microsoft.AspNetCore.Cryptography.KeyDerivation.PBKDF2.IPbkdf2Provider
    {
        public ManagedPbkdf2Provider() { }
        public byte[] DeriveKey(string password, byte[] salt, Microsoft.AspNetCore.Cryptography.KeyDerivation.KeyDerivationPrf prf, int iterationCount, int numBytesRequested) { throw null; }
    }

    internal sealed partial class Win7Pbkdf2Provider : Microsoft.AspNetCore.Cryptography.KeyDerivation.PBKDF2.IPbkdf2Provider
    {
        public Win7Pbkdf2Provider() { }
        public byte[] DeriveKey(string password, byte[] salt, Microsoft.AspNetCore.Cryptography.KeyDerivation.KeyDerivationPrf prf, int iterationCount, int numBytesRequested) { throw null; }
    }

    internal sealed partial class Win8Pbkdf2Provider : Microsoft.AspNetCore.Cryptography.KeyDerivation.PBKDF2.IPbkdf2Provider
    {
        public Win8Pbkdf2Provider() { }
        public byte[] DeriveKey(string password, byte[] salt, Microsoft.AspNetCore.Cryptography.KeyDerivation.KeyDerivationPrf prf, int iterationCount, int numBytesRequested) { throw null; }
    }
}
