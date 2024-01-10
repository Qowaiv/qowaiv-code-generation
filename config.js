module.exports = {
  platform: 'azure',
  endpoint: 'https://dev.azure.com/tjip/',
  token: process.env.TOKEN,
  prConcurrentLimit: 0,
  branchConcurrentLimit: 0,
  packageRules: [
    {
      groupName: "all non-major dependencies",
      groupSlug: "all-minor-patch",
      matchPackagePatterns: [
        "*"
      ],
      matchUpdateTypes: [
        "minor",
        "patch"
      ]
    }
  ],
  "ignorePaths": [
    "**/.tools/**",
    "**/apps/**",
    "**/packages/Hypotheken.Common.Authentication/**",
    "**/packages/Hypotheken.Common.Authorization/**",
    "**/packages/Hypotheken.Common.Documentation/**",
    "**/packages/Hypotheken.Common.Health/**",
    "**/packages/Hypotheken.Common.Logging/**",
    "**/packages/Hypotheken.Common.Messaging/**",
    "**/packages/Hypotheken.Common.Serialization/**",
    "**/packages/Hypotheken.Common.Storage/**",
    "**/packages/Hypotheken.Features/**",
    "**/packages/Hypotheken.HttpStandards/**",
    "**/unmanaged/**"
  ],
  ignoreDeps: [
    "Moq",
    "Microsoft.CodeAnalysis.Common",
    "Microsoft.CodeAnalysis.CSharp",
    "Microsoft.CodeAnalysis.Workspaces.Common"
  ],
  nuget: {
    registryUrls: [
      "https://api.nuget.org/v3/index.json",
      "https://pkgs.dev.azure.com/tjip/_packaging/Tjip-Components/nuget/v3/index.json",
      "https://pkgs.dev.azure.com/tjip/AbnAmro/_packaging/Hypotheek-NuGet/nuget/v3/index.json",
    ],
  },
  hostRules: [
    {
      hostType: 'nuget',
      matchHost: 'https://pkgs.dev.azure.com/tjip/_packaging/Tjip-Components/nuget/v3/index.json',
      username: 'tjip_nuget_feed_user',
      password: process.env.TJIP_NUGET_FEED_TOKEN,
    },
    {
      hostType: 'nuget',
      matchHost: 'https://pkgs.dev.azure.com/tjip/AbnAmro/_packaging/Hypotheek-NuGet/nuget/v3/index.json',
      username: 'ArtifactsDocker',
      password: process.env.TJIP_NUGET_FEED_TOKEN,
    },
    {
      hostType: 'nuget',
      matchHost: 'https://pkgs.dev.azure.com/tjip/',
      username: 'tjip_nuget_feed_user',
      password: process.env.TJIP_NUGET_FEED_TOKEN,
    },
  ],
  repositories: ['AbnAmro/Hypotheek'],
};