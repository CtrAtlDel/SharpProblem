FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /src
RUN apt update
RUN apt install -y protobuf-compiler
COPY . .
RUN cd CourseProject.Worker && dotnet publish -o /app

FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT [ "dotnet", "/app/CourseProject.Worker.dll" ]
