import React from "react";
import { observer } from "mobx-react-lite";
import { Search } from "semantic-ui-react";

const ActivitySearchBar = () => {
  return <Search noResultsDescription />;
};

export default observer(ActivitySearchBar);
