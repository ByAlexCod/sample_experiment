<template>
  <div id="app">

    <div v-if="errorMsg" class="error">
      <span class="bold">Error:</span> {{ errorMsg }}
    </div>

    <input type="text" name="username" v-model="username" />
    <input type="password" name="password" v-model="password" />
    <a href="#" @click="basicLogin(username, password)">Login</a>

  </div>
</template>

<script>
import ApplicationAuthService from './services/ApplicationAuthService'
import { AuthServiceConfiguration, ELevel } from '@signature/webfrontauth';

export default {
  name: 'app',
  data () {
    return {
      username: '',
      password: '',
      errorMsg: null,
      authInfo: null
    }
  },

  created: function () {
    ApplicationAuthService.instance.addOnChange(() => this.updateInfo());
  },

  methods: {
    updateInfo() {
      this.authInfo = ApplicationAuthService.instance.authenticationInfo;
    },

    basicLogin(username, password) {
      if (!(username || password) || !(username) || !(password))
        return this.errorMsg = 'Issue when attempting to login.';

      ApplicationAuthService.instance.basicLogin(username, password).then(_ => {
        console.log(this.authInfo);
      });
    }
  }
}
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}

h1, h2 {
  font-weight: normal;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  display: inline-block;
  margin: 0 10px;
}

a {
  color: #42b983;
}

.bold {
  font-weight: bold;
}

.error {
  margin: 10px;
  color: red;
}
</style>
