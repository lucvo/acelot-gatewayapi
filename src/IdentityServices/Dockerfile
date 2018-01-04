FROM microsoft/aspnetcore:2.0
ARG source
WORKDIR /app
EXPOSE 7000
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "IdentityServices.dll"]
