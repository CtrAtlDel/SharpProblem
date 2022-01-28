FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
WORKDIR /src
RUN apt update
RUN apt install -y protobuf-compiler
COPY . .
RUN cd CourseWork.Manager && dotnet publish -o /app

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT [ "dotnet", "/app/CourseWork.Manager.dll" ]