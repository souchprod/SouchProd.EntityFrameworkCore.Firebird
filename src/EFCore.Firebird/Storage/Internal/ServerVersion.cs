// Copyright (c) SouchProd. All rights reserved. // TODO: Credits Pomelo Foundation & EFCore
// Licensed under the MIT. See LICENSE in the project root for license information.

using System;
using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore.Storage.Internal
{
    public class ServerVersion
    {
        public static Regex ReVersion = new Regex(@"\d+\.\d+\.?(?:\d+)?");

        public ServerVersion(string versionString)
        {
            Type = versionString.ToLower().Contains("firebird") ? ServerType.Firebird : ServerType.Interbase;
            var semanticVersion = ReVersion.Matches(versionString);
            if (semanticVersion.Count > 0)
            {
                //if (Type == ServerType.Firebird && semanticVersion.Count > 1)
                //    Version = Version.Parse(semanticVersion[0].Value);
                //else
                    Version = Version.Parse(semanticVersion[0].Value);
            }
            else
            {
                throw new InvalidOperationException($"Unable to determine server version from version string '${versionString}'");
            }
        }

        public readonly ServerType Type;

        public readonly Version Version;

        public bool SupportsDateTime6 => Version >= new Version(2,1);

        public bool SupportsRenameIndex
        {
            get
            {
                if (Type == ServerType.Firebird)
                {
                    return Version >= new Version(2,1);
                }
                
                // TODO Awaiting feedback from Mariadb on when they will support rename index!
                return false;
            }
        }
    }

    public enum ServerType
    {
        Firebird,
        Interbase
    }
}