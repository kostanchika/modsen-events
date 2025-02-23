FROM node:23-alpine3.20

WORKDIR /app

COPY eventsreact/package.json eventsreact/package-lock.json ./

RUN npm install

COPY eventsreact/. .

RUN npm run build

CMD ["npm", "run", "preview"]