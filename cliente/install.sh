#!/bin/bash

# URL do arquivo zip no GitHub Releases
ZIP_URL="http://realbetis.software:8081/TrucoGoiano.zip"

# Nome do arquivo zip
ZIPFILE="TrucoGoiano.zip"

# Nome do arquivo executável
EXECUTABLE="linuxBuild/TrucoGoiano.x86_64"

# Nome do arquivo .desktop
DESKTOP_FILE="TrucoGoiano.desktop"

# Caminho de instalação (pode ser alterado conforme necessário)
INSTALL_DIR="$HOME/TrucoGoiano"

# Nome do ícone
ICON="icon.jpeg"

# Baixar o arquivo zip
echo "Baixando $ZIP_URL ..."
curl -L -o $ZIPFILE $ZIP_URL

# Verificar se o download foi bem-sucedido
if [ $? -ne 0 ]; then
    echo "Erro ao baixar o arquivo zip."
    exit 1
fi

# Criar diretório de instalação
mkdir -p $INSTALL_DIR

# Descompactar o arquivo zip no diretório de instalação
unzip -o $ZIPFILE -d $INSTALL_DIR

# Tornar o executável principal executável
chmod +x $INSTALL_DIR/$EXECUTABLE

# Criar o arquivo .desktop com caminho dinâmico
cat > $INSTALL_DIR/$DESKTOP_FILE << EOL
[Desktop Entry]
Version=1.0
Name=Truco Goiano
Comment=Executar o jogo Truco Goiano
Exec=$INSTALL_DIR/$EXECUTABLE
Icon=$INSTALL_DIR/$ICON
Terminal=false
Type=Application
EOL

# Tornar o arquivo .desktop executável
chmod +x $INSTALL_DIR/$DESKTOP_FILE

# Copiar o arquivo .desktop para /usr/share/applications para que apareça no menu de aplicativos
sudo cp $INSTALL_DIR/$DESKTOP_FILE /usr/share/applications/

# Copiar o arquivo .desktop para a área de trabalho do usuário
cp $INSTALL_DIR/$DESKTOP_FILE $HOME/Desktop/

# Limpar o arquivo zip baixado
rm $ZIPFILE

echo "Instalação concluída! Clique duas vezes no ícone Truco Goiano na área de trabalho para iniciar o jogo."

# Executar o jogo
$INSTALL_DIR/$EXECUTABLE
