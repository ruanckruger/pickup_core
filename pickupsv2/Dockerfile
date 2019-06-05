FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["pickupsv2/pickupsv2.csproj", "pickupsv2/"]
RUN dotnet restore "pickupsv2/pickupsv2.csproj"
COPY . .
WORKDIR "/src/pickupsv2"
RUN dotnet build "pickupsv2.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "pickupsv2.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "pickupsv2.dll"]